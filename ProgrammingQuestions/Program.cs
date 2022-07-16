/*1. What is REST
Representational state transfer is a software architectural style.
It is a subset of the HTTP Protocol GET, POST, PUT, DELETE
It creates stateless APIs'. The receiver does not retain session state from previous requests.
The term REST was introduced and defined in 2000 by Roy Fielding in his doctoral dissertation.
*/

/*2. Class vs Struct
A class is a blueprint for an object.
To create a struct the following characteristics should apply

It logically represents a single value, similar to primitive types (int, double, etc.).

It has an instance size under 16/20/24 bytes x32/x86/x64. (Microsoft violates this rule many times)
Instances of the type are large (greater than 16/20/24 bytes) and are not passed as method parameters or returned from methods.

It is immutable.

It will not have to be boxed frequently.
 */

/*3. Differences between stack and heap memory allocations
reference: https://docs.microsoft.com/en-us/archive/blogs/ericlippert/the-stack-is-an-implementation-detail-part-two

The idea is that there is a large block of memory reserved for instances of reference types. This block of memory can have “holes” – some of the memory is associated with “live” objects,
and some of the memory is free for use by newly created objects. Ideally though we want to have all the allocated memory bunched together and a large section of “free” memory at the top.

If we’re in that situation when new memory is allocated then the “high water mark” is bumped up, eating up some of the previously “free” portion of the block.
The newly-reserved memory is then usable for the reference type instance that has just been allocated. That is extremely cheap; just a single pointer move,
plus zeroing out the newly reserved memory if necessary.

If we have holes then we can maintain a “free list” – a linked list of holes. We can then search the free list for a hole of appropriate size and fill it in.
This is a bit more expensive since there is a list search. We want to avoid this suboptimal situation if possible.

When a garbage collection is performed there are three phases: mark, sweep and compact. In the “mark” phase, we assume that everything in the heap is “dead”.
The CLR knows what objects were “guaranteed alive” when the collection started, so those guys are marked as alive. Everything they refer to is marked as alive, and so on,
until the transitive closure of live objects are all marked. In the “sweep” phase, all the dead objects are turned into holes. In the “compact” phase,
the block is reorganized so that it is one contiguous block of live memory, free of holes.

The CLR collector is generational. Objects start off in the “short lived” heap.
If they survive they eventually move to the “medium lived” heap, and if they survive there long enough, they move to the “long lived” heap.
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
This explains why you cannot make a “ref int” field. If you could then you could store a ref to the value of a short-lived local inside a long-lived object. Were that legal then using the stack as a memory management technique would no longer be a viable optimization; value types would be just another kind of reference type and would have to be garbage collected.
Anonymous function closures and iterator block closures are implemented behind-the-scenes by turning locals and formal parameters into fields. So now you know why it is illegal to capture a ref or out formal parameter in an anonymous function or iterator block; doing so would not be a legal field.
Of course we do not want to have ugly and bizarre rules in the language like “you can close over any local or value parameter but not a ref or out parameter”. But because we want to be able to get the optimization of putting value types on the stack, we have chosen to put this odd restriction into the language. Design is, as always, the art of finding compromises.
Finally, the CLR does allow “ref return types”; you could in theory have a method “ref int M() { … }” that returned a reference to an integer variable. If for some bizarre reason we ever decided to allow that in C#, we’d have to fix up the compiler and verifier so that they ensured that it was only possible to return refs to variables that were known to be on the heap, or known to be “lower down” on the stack than the callee.
}
So there you go. Local variables of value type go on the stack because they can. They can because (1) “normal” locals have strict ordering of lifetime, and (2) value types are always copied by value and (3) it is illegal to store a reference to a local anywhere that could live longer than the local. By contrast, reference types have a lifetime based on the number of living references, are copied by reference, and those references can be stored anywhere. The additional flexibility that reference types give you comes at the cost of a much more complex and expensive garbage collection strategy.
But again, all of this is an implementation detail. Using the stack for locals of value type is just an optimization that the CLR performs on your behalf. The relevant feature of value types is that they have the semantics of being copied by value, not that sometimes their deallocation can be optimized by the runtime.
 */

/*4. What is a Garbage Collector
reference: https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals
In the common language runtime (CLR), the garbage collector (GC) serves as an automatic memory manager.
The garbage collector manages the allocation and release of memory for an application.
For developers working with managed code, this means that you don't have to write code to perform memory management tasks.
Automatic memory management can eliminate common problems,
such as forgetting to free an object and causing a memory leak or attempting to access memory for an object that's already been freed.
*/

/*5. What are the Garbage Collector Generations
 The GC algorithm is based on several considerations:

It's faster to compact the memory for a portion of the managed heap than for the entire managed heap.
Newer objects have shorter lifetimes and older objects have longer lifetimes.
Newer objects tend to be related to each other and accessed by the application around the same time.
Garbage collection primarily occurs with the reclamation of short-lived objects. To optimize the performance of the garbage collector, the managed heap is divided into three generations, 0, 1, and 2, so it can handle long-lived and short-lived objects separately. The garbage collector stores new objects in generation 0. Objects created early in the application's lifetime that survive collections are promoted and stored in generations 1 and 2. Because it's faster to compact a portion of the managed heap than the entire heap, this scheme allows the garbage collector to release the memory in a specific generation rather than release the memory for the entire managed heap each time it performs a collection.

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

/* 6. What is the .NET Common Language Runtime (CLR)
The heart of .NET Framework is a run-time environment called the common language runtime (CLR).
The CLR manages the life cycle and executes .NET applications (code).
It also provides services that make the development process easier.

As part of the Microsoft .NET Framework, the Common Language Runtime (CLR) is the programming (Virtual Machine component)
that manages the execution of programs written in any language that uses the .NET Framework, for example C#, VB.Net, F# and so on.
Programmers write code in any language, including VB.Net, C# and F#
when they compile their programs into an intermediate form of code called CLI in a portable execution file (PE)
that can be managed and used by the CLR and then the CLR converts it into machine code to be will executed by the processor.
The information about the environment, programming language,
its version and what class libraries will be used for this code are stored in the form of metadata with the compiler that tells the CLR how to handle this code.
The CLR allows an instance of a class written in one language to call a method of the class written in another language.
 */

/* 7. What is the .NET Framework
The .NET Framework is a software framework that enables developers to create applications that run on a variety of platforms.
The .NET Framework is a collection of software components that provide a common set of services for the development of applications.

The two major components of .NET Framework are the Common Language Runtime and the .NET Framework Class Library.

The Common Language Runtime (CLR) is the execution engine that handles running applications.
It provides services like thread management, garbage collection, type-safety, exception handling, and more.
The Class Library provides a set of APIs and types for common functionality.
It provides types for strings, dates, numbers, etc.
The Class Library includes APIs for reading and writing files, connecting to databases, drawing, and more.
.NET applications are written in the C#, F#, or Visual Basic programming language.
Code is compiled into a language-agnostic Common Intermediate Language (CIL).
Compiled code is stored in assemblies—files with a .dll or .exe file extension.

When an app runs, the CLR takes the assembly and uses a just-in-time compiler (JIT)
to turn it into machine code that can execute on the specific architecture of the computer it is running on.
 */

/* 8. Compiled vs Interpreted Languages
Compiled languages are converted directly into machine code that the processor can execute.
As a result, they tend to be faster and more efficient to execute than interpreted languages.
They also give the developer more control over hardware aspects, like memory management and CPU usage.

Compiled languages need a “build” step – they need to be manually compiled first.
You need to “rebuild” the program every time you need to make a change.
In our hummus example, the entire translation is written before it gets to you.
If the original author decides that he wants to use a different kind of olive oil,
the entire recipe would need to be translated again and resent to you.

Examples of pure compiled languages are C, C++, Erlang, Haskell, Rust, and Go.
------------------------------------------------------------------------------------
Interpreters run through a program line by line and execute each command.
Here, if the author decides he wants to use a different kind of olive oil,
he could scratch the old one out and add the new one.
Your translator friend can then convey that change to you as it happens.

Examples of common interpreted languages are PHP, Ruby, Python, and JavaScript.
 */

/* 9. out vs ref keyword
out allows methods to return more than one thing. This is useful when you want to return a value and a reference to a variable.
ref allows methods to return a reference to a variable. This is useful when you want to return a reference to a variable.
 */

/* 10. const vs readonly
const are used to declare variables that are immutable.
This means that the value of the variable cannot change after it is initialized.
readonly are used to declare variables that are read-only.
The difference is that const is initialized at compile time while readonly is initialized at runtime
 */

/*11. What is an idempotent operation
 No matter how many times you call the operation, the result will be the same.
*/

/* 12. Interface vs Abstract Class
Interfaces have multiple inheritance - a class can implement multiple interfaces.
Abstract classes cannot

An abstract class can contain non abstract methods.
Since C# 8.0 Interfaces can contain concrete methods. Note that classes that implement that interface, do not inherit
the implemented methods. Diamond inheritance is resolve explicitly in the class.

In an abstract class we can have ctors while in interfaces we cannot

Since C# 8.0, interfaces permit access modifiers.
*/

/* 13. What is the Main Thread 
C# provides built-in support for multithreaded programming.
A multi-threaded program contains two or more parts that can run concurrently.
Each part of such a program is called a thread and each thread defines a separate path of execution.

When a C# program starts up, one thread begins running immediately. This is called the main thread of our program.
 */

/* 14. What is the repository pattern

 */

/*15. Is the repository pattern useful when using entity framework?
https://www.thereformedprogrammer.net/is-the-repository-pattern-useful-with-entity-framework-core/
*/

/* 16. What is IDisposable and when to use it
The primary use of this interface is to release unmanaged resources.
The garbage collector automatically releases the memory allocated to a managed object when that object is no longer used.
However, it is not possible to predict when garbage collection will occur.
Furthermore, the garbage collector has no knowledge of unmanaged resources such as window handles, or open files and streams.

Use the Dispose method of this interface to explicitly release unmanaged resources in conjunction with the garbage collector.
The consumer of an object can call this method when the object is no longer needed.
 */

Console.WriteLine("Hello Learner! I hope this solution helps you become better!");
