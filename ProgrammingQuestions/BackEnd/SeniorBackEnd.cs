/*3. Differences between stack and heap memory allocations
reference: https://docs.microsoft.com/en-us/archive/blogs/ericlippert/the-stack-is-an-implementation-detail-part-two

The idea is that there is a large block of memory reserved for instances of reference types.
This block of memory can have “holes” – some of the memory is associated with “live” objects,
and some of the memory is free for use by newly created objects.
Ideally though we want to have all the allocated memory bunched together and a large section of “free” memory at the top.

If we’re in that situation when new memory is allocated then the “high water mark” is bumped up,
eating up some of the previously “free” portion of the block.
The newly-reserved memory is then usable for the reference type instance that has just been allocated.
That is extremely cheap; just a single pointer move,
plus zeroing out the newly reserved memory if necessary.

If we have holes then we can maintain a “free list” – a linked list of holes.
We can then search the free list for a hole of appropriate size and fill it in.
This is a bit more expensive since there is a list search. We want to avoid this suboptimal situation if possible.

When a garbage collection is performed there are three phases: mark, sweep and compact.
In the “mark” phase, we assume that everything in the heap is “dead”.
The CLR knows what objects were “guaranteed alive” when the collection started, so those guys are marked as alive.
Everything they refer to is marked as alive, and so on,
until the transitive closure of live objects are all marked.
In the “sweep” phase, all the dead objects are turned into holes. In the “compact” phase,
the block is reorganized so that it is one contiguous block of live memory, free of holes.

The CLR collector is generational. Objects start off in the “short lived” heap.
If they survive they eventually move to the “medium lived” heap, and if they survive there long enough,
they move to the “long lived” heap.
The GC runs very often on the short lived heap and very seldom on the long lived heap;
the idea is that we do not want to have the expense of constantly re-checking a long-lived object to see if it is still alive.
But we also want short-lived objects to be reclaimed swiftly.

The GC has a huge amount of carefully tuned policy that ensures high performance;
it attempts to balance the memory and time costs of having a Swiss-cheesed heap against the high cost of the compaction phase.
Extremely large objects are stored in a special heap that has different compaction policy. And so on. I don’t know all the details, and fortunately,
I don’t need to. (And of course, I have left out lots of additional complexity that is not germane to this article – pinning and finalization and weak refs and so on.)

Now compare this to the stack. The stack is like the heap in that it is a big block of memory with a “high water mark”.
But what makes it a “stack” is that the memory on the bottom of the stack always lives longer than the memory on the top of the stack;
the stack is strictly ordered. The objects that are going to die first are on the top, the objects that are going to die last are on the bottom.
And with that guarantee, we know that the stack will never have holes, and therefore will not need compacting.
We know that the stack memory will always be “freed” from the top, and therefore do not need a free list. We know that anything low-down on the stack is guaranteed alive,
and so we do not need to mark or sweep.

On a stack, the allocation is just a single pointer move – the same as the best (and typical) case on the heap.
But because of all those guarantees, the deallocation is also a single pointer move! And that is where the huge time performance savings is.
A lot of people seem to think that heap allocation is expensive and stack allocation is cheap. They are actually about the same, typically.
It’s the deallocation costs – the marking and sweeping and compacting and moving memory from generation to generation – that are massive for heap memory compared to stack memory.

Clearly it is better to use a stack than a GC heap if you can. When can you?
Only when you can guarantee that all the necessary conditional that make a stack work are actually achieved.
Local variables and formal parameters of value type are the sweet spot that achieve that.
The locals of frames on the bottom of the stack clearly live longer than the locals on the frames of the top of the stack.
Locals of value type are copied by value, not by reference, so the local is the only thing that references the memory; 
there is no need to track who is referencing a particular value to determine its lifetime. And the only way to take a ref to a local is to pass it as a ref or out parameter,
which just passes the ref on up the stack. The local is going to be alive anyway, so the fact that there is a ref to it “higher up” the call stack doesn’t change its lifetime.

aside
{
A few asides:
This explains why you cannot make a “ref int” field.
If you could then you could store a ref to the value of a short-lived local inside a long-lived object.
Were that legal then using the stack as a memory management technique would no longer be a viable optimization;
value types would be just another kind of reference type and would have to be garbage collected.
Anonymous function closures and iterator block closures are implemented behind-the-scenes
by turning locals and formal parameters into fields.
So now you know why it is illegal to capture a ref or out formal parameter in an anonymous function or iterator block;
doing so would not be a legal field.
Of course we do not want to have ugly and bizarre rules in the language like
“you can close over any local or value parameter but not a ref or out parameter”.
But because we want to be able to get the optimization of putting value types on the stack,
we have chosen to put this odd restriction into the language. Design is, as always, the art of finding compromises.
Finally, the CLR does allow “ref return types”;
you could in theory have a method “ref int M() { … }” that returned a reference to an integer variable.
If for some bizarre reason we ever decided to allow that in C#,
we’d have to fix up the compiler and verifier so that they ensured that it was only possible
to return refs to variables that were known to be on the heap, or known to be “lower down” on the stack than the callee.
}
So there you go. Local variables of value type go on the stack because they can.
They can because (1) “normal” locals have strict ordering of lifetime,
and (2) value types are always copied by value 
and (3) it is illegal to store a reference to a local anywhere that could live longer than the local.
By contrast, reference types have a lifetime based on the number of living references,
are copied by reference, and those references can be stored anywhere.
The additional flexibility that reference types give you comes at the cost of a much more complex
and expensive garbage collection strategy.
But again, all of this is an implementation detail.
Using the stack for locals of value type is just an optimization that the CLR performs on your behalf.
The relevant feature of value types is that they have the semantics of being copied by value,
not that sometimes their deallocation can be optimized by the runtime.
 */

/*5. What are the Garbage Collector Generations
 The GC algorithm is based on several considerations:

It's faster to compact the memory for a portion of the managed heap than for the entire managed heap.
Newer objects have shorter lifetimes and older objects have longer lifetimes.
Newer objects tend to be related to each other and accessed by the application around the same time.
Garbage collection primarily occurs with the reclamation of short-lived objects.
To optimize the performance of the garbage collector, the managed heap is divided into three generations, 0, 1, and 2, so it can handle long-lived and short-lived objects separately.
The garbage collector stores new objects in generation 0.
Objects created early in the application's lifetime that survive collections are promoted and stored in generations 1 and 2.
Because it's faster to compact a portion of the managed heap than the entire heap, this scheme allows the garbage collector to release the memory in a specific generation rather than release the memory for the entire managed heap each time it performs a collection.

Generation 0. This is the youngest generation and contains short-lived objects.
An example of a short-lived object is a temporary variable. Garbage collection occurs most frequently in this generation.

Newly allocated objects form a new generation of objects and are implicitly generation 0 collections.
However, if they are large objects, they go on the large object heap (LOH), which is sometimes referred to as generation 3.
Generation 3 is a physical generation that's logically collected as part of generation 2.

Most objects are reclaimed for garbage collection in generation 0 and don't survive to the next generation.

If an application attempts to create a new object when generation 0 is full,
the garbage collector performs a collection in an attempt to free address space for the object.
The garbage collector starts by examining the objects in generation 0 rather than all objects in the managed heap.
A collection of generation 0 alone often reclaims enough memory to enable the application to continue creating new objects.

Generation 1. This generation contains short-lived objects and serves as a buffer between short-lived objects and long-lived objects.

After the garbage collector performs a collection of generation 0,
it compacts the memory for the reachable objects and promotes them to generation 1.
Because objects that survive collections tend to have longer lifetimes, it makes sense to promote them to a higher generation.
The garbage collector doesn't have to reexamine the objects in generations 1 and 2 each time it performs a collection of generation 0.

If a collection of generation 0 does not reclaim enough memory for the application to create a new object,
the garbage collector can perform a collection of generation 1, then generation 2.
Objects in generation 1 that survive collections are promoted to generation 2.

Generation 2. This generation contains long-lived objects.
An example of a long-lived object is an object in a server application that contains static data that's live for the duration of the process.

Objects in generation 2 that survive a collection remain in generation 2 until they are determined to be unreachable in a future collection.

Objects on the large object heap (which is sometimes referred to as generation 3) are also collected in generation 2.

Garbage collections occur on specific generations as conditions warrant.
Collecting a generation means collecting objects in that generation and all its younger generations.
A generation 2 garbage collection is also known as a full garbage collection,
because it reclaims objects in all generations (that is, all objects in the managed heap).

Objects that are not reclaimed in a garbage collection are known as survivors and are promoted to the next generation:

Objects that survive a generation 0 garbage collection are promoted to generation 1.
Objects that survive a generation 1 garbage collection are promoted to generation 2.
Objects that survive a generation 2 garbage collection remain in generation 2.
When the garbage collector detects that the survival rate is high in a generation,
it increases the threshold of allocations for that generation.
The next collection gets a substantial size of reclaimed memory.
The CLR continually balances two priorities:
1) not letting an application's working set get too large by delaying garbage collection and
2) not letting the garbage collection run too frequently.
 */

/* 17. Differences between Func - Action - Predicate 
Action is a delegate (pointer) to a method, that takes zero, one or more input parameters, but does not return anything.

Func is a delegate (pointer) to a method, that takes zero, one or more input parameters, and returns a value (or reference).

Predicate is a special kind of Func often used for comparisons (takes a generic parameter and returns bool). 

 */

/* 18. What is the difference between Span<T> and Memory<T>
 
Span<T> and Memory<T> are both types in C# that represent a contiguous region of memory.
However, they are used for slightly different purposes.

Span<T> is a lightweight, stack-allocated type that can be used to represent a span of memory,
such as a section of an array or a substring of a string.
It is intended to be used in situations where performance is critical,
such as in high-performance code or in situations where the lifetime of the memory is well-defined.

Memory<T> is heap-allocated and can be used to represent a region of memory that may have a longer lifetime
than the function that created it.
It is intended to be used in situations where the lifetime of the memory is less well-defined,
such as when working with asynchronous code or when passing memory between threads.

In summary, Span<T> is designed for high-performance scenarios where the lifetime of the memory is well defined,
whereas Memory<T> is designed for scenarios where the lifetime of the memory is less well defined, such as async code.
 */