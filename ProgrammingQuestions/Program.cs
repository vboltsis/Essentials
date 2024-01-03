/*1. What is REST
Representational state transfer is a software architectural style.
It is a subset of the HTTP Protocol GET, POST, PUT, DELETE, PATCH, OPTIONS
It creates stateless APIs'. The receiver does not retain session state from previous requests.
The term REST was introduced and defined in 2000 by Roy Fielding in his doctoral dissertation.
*/

/*2. Class vs Struct vs Interface
A class is a blueprint for an object.
A struct is also a blueprint for an object but it is a value type and not a reference type.
To create a struct the following characteristics should apply

It logically represents a single value, similar to primitive types (int, double, etc.).

It has an instance size under 16/20/24 bytes x32/x86/x64. (Microsoft violates this rule many times)
Instances of the type are large (greater than 16/20/24 bytes) and
are not passed as method parameters or returned from methods. This is to prevent excessive copying of the struct.

It is immutable.

It will not have to be boxed frequently.

An interface is a set of related methods/properties that a class or struct can implement.
An interface defines the signature of the methods, but not their implementation.
This allows you to create a common set of behaviors that can be shared by multiple classes or structs.

for some odd interface cases you can find more on: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/interfaces/explicit-interface-implementation
 */

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
It is an abstraction of the data layer.
A Repository mediates between the domain and data mapping layers, acting like an in-memory domain object collection.
Client objects construct query specifications declaratively and submit them to Repository for satisfaction.
Objects can be added to and removed from the Repository, as they can from a simple collection of objects,
and the mapping code encapsulated by the Repository will carry out the appropriate operations behind the scenes.
Conceptually, a Repository encapsulates the set of objects persisted in a data store and the operations performed over them,
providing a more object-oriented view of the persistence layer.
Repository also supports the objective of achieving a clean separation and one-way dependency between the domain and data mapping layers.
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

/* 19. What are threads and what is the difference with async in c#
Threads are a way for a program to split itself into two or more simultaneously (concurrently) running tasks.
This is known as multithreading. Each thread has its own stack and program counter,
but shares memory with other threads in the same process.

Async and await are keywords in C# that allow a program to run asynchronous code.
Asynchronous code is code that runs independently of the main program flow,
allowing the program to continue executing other code while the asynchronous code is running.
The await keyword is used to suspend the execution of the current method until the awaited task is complete.
The async keyword is used to indicate that a method contains asynchronous code.

In short, threading is a way to run multiple tasks at the same time,
while async/await is a way to run a single task in a non-blocking way.
 */

/* 20. What are Green threads
Green threads are a type of lightweight threads that are scheduled and managed by a user-space library or language runtime,
rather than the operating system's kernel.
Unlike kernel threads, which are native to the operating system and rely on the kernel for scheduling and management,
green threads can be created and managed entirely within user-space.

Green threads offer several advantages over traditional kernel threads,
including the ability to run on a single core, lighter weight context switching,
and the ability to avoid the overhead associated with creating and managing threads in the operating system.
However, because green threads rely on cooperative scheduling, they cannot take advantage of multiple cores,
and they can also block the entire process if one green thread blocks.

In general, green threads are used in situations where a large number of lightweight,
cooperative tasks need to be scheduled and managed efficiently,
such as in server applications where a large number of connections need to be handled simultaneously. 
They are also used in scripting languages and other environments where the overhead of creating
and managing kernel threads would be prohibitive.
 */

/* 21. Ways to do zero downtime deployments
Deployment across multiple instances: By deploying the new version of the software to multiple instances,
you can gradually shift traffic from the old instances to the new ones.
This way, you can ensure that there is no single point of failure and that users are not impacted by the deployment.

Use blue-green deployment: Blue-green deployment involves having two instances of the application running simultaneously,
one in the "blue" state and one in the "green" state. You can deploy the new version of the software to the green instance,
test it, and then switch traffic over to it, making it the new blue instance.

Use a load balancer: A load balancer can help distribute traffic between multiple instances,
making it easy to redirect traffic during a deployment.
You can also use a load balancer to perform health checks on instances and redirect traffic away from instances
that are not functioning properly.

Implement versioning: Implement versioning in your code so that different versions of the software can coexist
and be deployed to different instances.
This allows you to gradually phase out old versions of the software as you deploy new ones.

Use feature flags: Feature flags allow you to roll out new features gradually by only enabling them for a subset of users.
This allows you to deploy new software versions with confidence, knowing that you can quickly roll back changes if necessary.
 */

/* 22. What is the difference between pinned object heap and frozen object heap in .NET
In .NET, the terms "pinned object heap" and "frozen object heap" refer to two different memory regions
that serve different purposes.

Pinned Object Heap:
The Pinned Object Heap is a region of memory in the .NET runtime that is used to store objects that have been explicitly pinned
using the GCHandle struct. Pinning is a technique used to prevent the garbage collector from moving an object in memory,
which can be necessary for interoperability with unmanaged code or for optimizing performance in certain scenarios.
When an object is pinned, its memory address is fixed, and the garbage collector will not attempt to move it.

Frozen Object Heap:
The Frozen Object Heap is a memory region used by the .NET runtime to store objects
that are not expected to change during the lifetime of the application.
This memory region is used by the runtime for application domain unloading,
as it allows the runtime to simply discard the entire memory region
without having to scan the objects for potential references to other objects in the application domain.

In summary, the main difference between the Pinned Object Heap and the Frozen Object Heap
is that the former is used for objects that must be prevented from moving in memory,
while the latter is used for objects that are not expected to change and can be discarded as a whole
when the application domain is unloaded.

In terms of performance, neither the Pinned Object Heap nor the Frozen Object Heap is inherently better than the other.
They serve different purposes and are optimized for different scenarios.
*/

/* 23. What is the difference between loader heap and process heap in .NET
In .NET, the terms "Loader Heap" and "Process Heap" refer to two different memory regions used by the .NET runtime.

Loader Heap:
The Loader Heap is a region of memory used by the .NET runtime to store data structures
and code required for loading and executing managed assemblies.
This memory region is created when the .NET runtime is loaded into a process
and is used to store a variety of data structures, including the module table, type table, method table, and static field data.

Process Heap:
The Process Heap, also known as the default heap,
is a region of memory used by the operating system to allocate memory for the application.
When an application requests memory using functions such as malloc or new,
the operating system uses the Process Heap to fulfill the request.

In .NET, the garbage collector manages the memory used by managed objects,
which are allocated on the Process Heap.
The Process Heap is also used for other runtime data structures, such as thread stacks and thread-local storage.

In summary, the Loader Heap is a memory region used by the .NET runtime to store data structures
and code required for loading and executing managed assemblies,
while the Process Heap is a memory region used by the operating system to allocate memory
for the application and is used to store all other objects allocated by the application. 
*/

/* 24. How is Gen 0 related with the process heap and the loader heap in .NET
In .NET, the garbage collector manages memory by dividing it into several generations.
The first generation, Gen 0, is the youngest generation and is where newly allocated objects are initially placed.
Gen 0 is related to both the process heap and the loader heap in the following ways:

Process Heap:
When an object is allocated in .NET, it is allocated on the process heap,
which is a region of memory used by the operating system to allocate memory for the application.
The garbage collector manages the process heap by periodically running garbage collection,
which identifies objects that are no longer in use and frees up their memory.

Gen 0 objects are allocated on the process heap, along with objects in the other generations.
When a Gen 0 collection occurs, the garbage collector only scans objects in Gen 0 for garbage collection.
This makes Gen 0 collections faster than collections for objects in older generations.

Loader Heap:
The loader heap is a region of memory used by the .NET runtime to store data structures
and code required for loading and executing managed assemblies.
When an assembly is loaded into the .NET runtime, its metadata is loaded into the loader heap.

Gen 0 collections can also impact the loader heap, as they can trigger the loading of new metadata into the heap.
When a new object is allocated on the process heap,
its type information is loaded into the loader heap, which can cause the heap to grow.
When a Gen 0 collection occurs, any unused type information may be removed from the loader heap, which can reduce its size.

In summary, Gen 0 objects are allocated on the process heap,
and Gen 0 collections only scan objects in Gen 0 for garbage collection.
The loader heap is used by the .NET runtime to store data structures
and code required for loading and executing managed assemblies,
and Gen 0 collections can impact the loader heap by triggering the loading of new metadata into the heap.
 */

/* 25. What is the difference between a managed and unmanaged heap in .NET
In .NET, the terms "managed heap" and "unmanaged heap" refer to two different memory regions used by the .NET runtime.

Managed Heap:
The managed heap is a region of memory used by the .NET runtime to store managed objects.
Managed objects are objects whose lifetime is managed by the .NET runtime,
and they are allocated on the managed heap using the new keyword.

Unmanaged Heap:
The unmanaged heap is a region of memory used by the .NET runtime to store unmanaged objects.
Unmanaged objects are objects whose lifetime is not managed by the .NET runtime,
and they are allocated on the unmanaged heap using the malloc function.

In summary, the managed heap is a region of memory used by the .NET runtime to store managed objects,
while the unmanaged heap is a region of memory used by the .NET runtime to store unmanaged objects.
*/

/* 26. What is the difference between a managed and unmanaged stack in .NET
In .NET, the terms "managed stack" and "unmanaged stack" refer to two different types of stacks that are used by the runtime.

Managed Stack:
The managed stack is a region of memory used by the .NET runtime to store information related to the execution of managed code.
The managed stack is used to store method parameters, local variables,
and other information related to the execution of managed code.
When a method is called, a new frame is created on the managed stack to store information related to that method's execution.

The managed stack is managed by the .NET runtime and is subject to automatic memory management through the garbage collector.

Unmanaged Stack:
The unmanaged stack, also known as the native stack, is a region of memory used by native code executed by the .NET runtime.
When the .NET runtime interoperates with unmanaged code,
such as a C++ library, the unmanaged stack is used to store method parameters,
local variables, and other information related to the execution of that code.

Unlike the managed stack, the unmanaged stack is not managed by the .NET runtime
and is not subject to automatic memory management through the garbage collector.
Instead, the responsibility for managing the unmanaged stack lies with the developer of the native code.

In summary, the managed stack is used to store information related to the execution of managed code,
while the unmanaged stack is used to store information related to the execution of native code.
The managed stack is managed by the .NET runtime and is subject to automatic memory management through the garbage collector,
while the unmanaged stack is not managed by the .NET runtime and is the responsibility of the developer of the native code.
*/

/* 27. What are secondary reads in SQL
In SQL, "secondary reads" usually refer to additional reads of data that occur when a query requires data
that is not already present in memory or cached by the database.
These additional reads can slow down query performance,
especially when the data being read is stored on a separate physical disk or in a remote location.

Secondary reads can occur in a number of situations, including:
-When a query joins data from multiple tables, and the necessary data is not already in memory.
-When a query requires data that is not covered by an index, and the data must be read from disk.
-When a query retrieves a large number of rows, and the database needs to access additional data blocks from disk.

To improve query performance and minimize the number of secondary reads,
it is often a good idea to use indexing and other optimization techniques.
This can help ensure that the necessary data is already in memory,
and that the database can retrieve it quickly without needing to perform additional reads. 
 */

/* 28. What is the difference between a thread and a task in .NET
 * 
In the .NET framework, a thread and a task are both means of achieving multi-threading,
but they are used in slightly different ways and for slightly different purposes.

***Thread***
In .NET, a thread represents an independent execution path.
It's a lower-level construct for multi-threading that provides a lot of control but also requires more management.
You typically use threads when you need a high degree of control over the threading process.
When you create a new thread, it takes resources like memory to store the thread's local variables,
stack, etc., which can be costly if not managed carefully.
In addition, working directly with threads means you need to handle things like thread pooling and synchronization,
which can add complexity to your code.

***Task***
Tasks, on the other hand, are part of the Task Parallel Library (TPL) and represent an asynchronous operation.
Tasks are generally easier to work with and more high-level than threads.
They are used to make writing concurrent and multi-threaded software easier.
When you start a task, it uses a thread from the thread pool instead of creating a new one, 
which is more efficient because it reuses threads and reduces the overhead of thread creation.
Tasks also allow for easier composition of asynchronous operations thanks to methods like
ContinueWith and WaitAll, and they can return a result through the Task<T> class.
In addition, tasks have built-in support for cancellation through the CancellationToken mechanism and
for handling exceptions.

In the .NET Framework and .NET Core, tasks don't directly spawn threads, and threads don't directly run tasks.

***Tasks and Threads***

When you start a Task using Task.Run or Task.Factory.StartNew, the Task scheduler assigns it to run on a thread.
However, by default, it doesn't create a new thread for each Task. Instead, it uses threads from a thread pool.
This approach is more efficient because creating a new thread is a costly operation in terms of system resources.

Moreover, a Task can move between threads over its lifetime,
particularly when it involves I/O-bound operations (like reading a file or making a network request).
While a Task is waiting for an I/O operation to complete, it can free up its thread to be used by another Task.
This behavior allows for more efficient use of system resources than dedicating a single thread to a single operation from start to finish,
especially for I/O-bound operations.

***Threads Running Tasks***

The .NET runtime manages threads and the assignment of tasks to threads.
A developer generally doesn't have direct control over which thread runs a given Task (except in some advanced scenarios).
Instead, when a Task is scheduled, it is placed on a queue.
The thread pool then takes tasks from the queue and executes them on available threads.

In summary, tasks and threads in .NET have a many-to-many relationship.
A single task can be run on multiple threads over its lifetime (though not simultaneously),
and a single thread can run multiple tasks (one after the other).
The .NET runtime manages this process to optimize system resources.

Here's a summary of the differences:
Thread is a lower-level concept for multi-threading that provides more control but requires more management.
Creating a new thread is a costly operation.

Task is a higher-level concept that represents an asynchronous operation.
It is more efficient because it uses a thread from the thread pool instead of creating a new one,
and it offers easier ways to compose and manage asynchronous operations.

As a general guideline, unless you specifically need the low-level control that threads offer,
you should use tasks in modern .NET applications because of their efficiency and ease of use.
*/

/* 29. How many design patterns exist and give examples of some of them
Design patterns are typical solutions to commonly occurring problems in software design. They are categorized into three primary groups:

~~Creational Patterns: These deal with object creation mechanisms, trying to create objects in a manner suitable to the situation. The basic form of object creation could result in design problems or add complexity to the design.
Creational design patterns solve this problem by somehow controlling this object creation.

Examples:
-Singleton Pattern: Ensures a class has only one instance and provides a global point of access to it.
-Factory Method: Defines an interface for creating an object, but lets subclasses alter the type of objects that will be created.
-Abstract Factory: Provides an interface for creating families of related or dependent objects without specifying their concrete classes.
-Builder: Allows constructing complex objects step by step. The pattern allows producing different types and representations of an object using the same construction code.
-Prototype: Allows copying existing objects without making your code dependent on their classes.

~~Structural Patterns: These concern class and object composition.
They use inheritance to compose interfaces and define ways to compose objects to obtain new functionalities.

Examples:
-Adapter Pattern: Allows objects with incompatible interfaces to collaborate.
-Decorator: Allows for the dynamic addition of behaviors to objects without affecting other objects from the same class.
-Composite: Composes objects into tree structures to represent part-whole hierarchies. Composite lets clients treat individual objects and compositions of objects uniformly.
-Proxy: Provides a placeholder for another object to control access, reduce cost, and reduce complexity.
-Flyweight: Minimizes memory usage or computational expenses by sharing as much as possible with similar objects.

~~Behavioral Patterns: These are concerned with algorithms and the assignment of responsibilities between objects.
They don't just describe patterns of objects or classes but also the patterns of communication between them.

Examples:
-Strategy: Allows defining a family of algorithms, encapsulating each one, and making them interchangeable. Strategy lets the algorithm vary independently from clients that use it.
-Observer: Defines a dependency between objects so that when one object changes state, all its dependents are notified and updated automatically.
-Command: Turns a request into a stand-alone object that contains all information about the request. This transformation lets you parameterize methods with different requests, delay or queue a request's execution, and support undoable operations.
-Iterator: Provides a way to access the elements of an aggregate object sequentially without exposing its underlying representation.
-Memento: Allows saving and restoring the previous state of an object without revealing the details of its implementation.
*/

/* 30. What are Sealed classes and how to extend a sealed class
Sealed classes in C# are used to prevent a class from being inherited.
In other words, if you declare a class as sealed, no other class can derive from it.
This is useful when you want to provide a comprehensive set of functionalities within a class and ensure that they remain unaltered by inheritance,
which could otherwise potentially alter the way the class is intended to be used.

However, you can still extend the functionality of a sealed class using a few different techniques:

Composition: Instead of inheriting from the sealed class, you can create a new class that includes it as a field,
sometimes referred to as a "has-a" relationship.

public class MyComposedClass
{
    private MySealedClass mySealedClass = new MySealedClass();
    
    // You can expose the functionalities of MySealedClass through MyComposedClass methods
}

Extension Methods: C# allows you to add new methods to an existing class using extension methods.
These are static methods in a static class, where the this keyword is used to specify which type the method operates on.

public static class MySealedClassExtensions
{
    public static void ExtendedMethod(this MySealedClass mySealedClass)
    {
        // New functionality here
    }
}


Aggregation: Similar to composition, but the relationship is typically looser.
The new class stores a reference to an instance of the sealed class rather than owning it directly.

public class MyAggregatedClass
{
    private MySealedClass mySealedClass;
    
    public MyAggregatedClass(MySealedClass mySealedClass)
    {
        this.mySealedClass = mySealedClass;
    }
    
    // Additional methods and properties
}

*/

/* 31. What is the difference between virtual and abstract methods in C#

In C#, both virtual and abstract methods enable polymorphism,
but they serve different purposes and have different rules for how they can be used in your classes:

Virtual Methods:

-Purpose: Virtual methods are used in a base class to define a method that can be overridden in derived classes. They provide a default implementation that may be sufficient for some derived classes, but can also be tailored in others by overriding it.
-Usage: A virtual method must provide an implementation in the base class. It is marked with the virtual keyword.
-Overriding: Derived classes can override the virtual method to provide a new implementation, but it's not mandatory. If not overridden, the default implementation in the base class is used.
-Instantiation: Classes with virtual methods can be instantiated, and they are not required to be derived from.

public class BaseClass
{
    public virtual void VirtualMethod()
    {
        // Default implementation
    }
}

Abstract Methods:

-Purpose: Abstract methods define a method signature without any implementation and must be overridden in non-abstract derived classes. They are used when the base class cannot provide a meaningful implementation for the method, and it is up to the derived class to provide the specific behavior.
-Usage: An abstract method cannot have any implementation in the base class and must be marked with the abstract keyword. Also, the class that contains an abstract method must be declared as abstract.
-Overriding: Derived classes must provide an implementation for abstract methods unless they are also declared abstract. Failure to do so will result in a compile-time error.
-Instantiation: Abstract classes cannot be instantiated directly; they must be subclassed, and their abstract methods must be implemented before you can create an instance of the derived class.

public abstract class AbstractClass
{
    public abstract void AbstractMethod();
}

*/

/* 32. What are Immutable Types in C#
Immutable types in C# are types whose instances cannot be changed once they have been created.
After an object of an immutable type is instantiated, its data cannot be altered in any way.
Any operation that appears to change the object actually returns a new object with the modified values.

Here are some characteristics and benefits of immutable types:

Unchangeable State: Once an instance of an immutable type is created, its state cannot change. If you want to modify an object, you would create a new instance with the new values.
Simplicity: Immutable objects are simpler to understand and use because you don't have to deal with complex state management. Once created, the state of the object is known and predictable at all times.
Thread Safety: Immutable objects are inherently thread-safe, as they cannot be modified after creation. This means there is no need to synchronize access to them across threads, which can lead to performance benefits in concurrent applications.
Good for Caching: Since immutable objects cannot change, they are ideal for caching. If you need the same data again, you can reuse the instance without worrying about whether it has been changed elsewhere in the code.
Hash Key Friendly: Immutable types make excellent dictionary keys because their hash codes do not change over time, ensuring that the hash-based collections work correctly.

In C#, strings are a common example of an immutable type.
When you "modify" a string, you are actually creating a new string object in memory.

To create your own immutable type in C#, you would:

Declare all fields as readonly.
Set all properties to have private setters or no setters at all.
Ensure that all properties return data that is also immutable.
Provide a constructor that sets all properties at the time of object creation.
Here’s an example of a simple immutable class:

public class ImmutablePoint
{
    public int X { get; }
    public int Y { get; }

    public ImmutablePoint(int x, int y)
    {
        X = x;
        Y = y;
    }

    public ImmutablePoint WithX(int x) => new ImmutablePoint(x, this.Y);
    public ImmutablePoint WithY(int y) => new ImmutablePoint(this.X, y);
}

*/

/* 33. What is RabbitMQ
RabbitMQ is an open-source message broker software (sometimes called message-oriented middleware)
that implements the Advanced Message Querying Protocol (AMQP).
The main purpose of RabbitMQ is to provide a central platform to send and receive messages,
and it can be used to handle background jobs or inter-service communication in a distributed system.

Here are some key concepts in RabbitMQ:

-Producer: A producer is an application that sends messages to a RabbitMQ broker.
-Consumer: A consumer is an application that receives messages from a RabbitMQ broker.
-Queue: A queue is a buffer that stores messages. Consumers connect to the queue to receive messages.
-Exchange: An exchange is responsible for routing messages to one or more queues.
The exchange type determines how it routes messages.
-Binding: A binding is a link between a queue and an exchange.

RabbitMQ has several exchange types that define how messages are routed:

-Direct Exchange: The message is routed to the queues whose binding key exactly matches the routing key of the message.
-Fanout Exchange: When a message is published to a fanout exchange, it is copied and sent to all queues bound to that exchange, ignoring any binding keys.
-Topic Exchange: The message is routed to one or many queues based on a matching between a message routing key and the pattern that was used to bind a queue to an exchange.
-Headers Exchange: The message is routed to queues based on header values instead of routing keys.
-Default Exchange: A direct exchange with no name (empty string) pre-declared by the broker. It is a direct exchange and the routing key is the queue name it should route to.

Here are some additional concepts you should be familiar with:

-Message Acknowledgement: When a message is delivered to a consumer, it must acknowledge the message.
If the consumer fails to do so, RabbitMQ will re-queue the message and may deliver it to another consumer.
-Message Durability: Messages can be set as persistent, and queues can be declared durable,
which means that they survive broker restarts.
-Prefetch Count: This setting controls how many messages the server will deliver to consumers before acknowledgments are received.
It is used to control the message flow.
-Dead Letter Exchange: When a message can't be processed or delivered, it can be sent to a dead letter exchange.
-Clustering and High Availability: RabbitMQ can be run in a cluster to ensure high availability and reliability.
-Federation and Shovel: These are mechanisms to connect multiple brokers (or clusters) to share messages between them.
 
 */

//Cool links
//https://endjin.com/blog/2020/09/arraypool-vs-memorypool-minimizing-allocations-ais-dotnet
//https://github.com/Maoni0/mem-doc/blob/master/doc/.NETMemoryPerformanceAnalysis.md
//https://www.youtube.com/watch?v=Z8M2BTscoD4
Console.WriteLine("Hello Learner! I hope this solution helps you become better!");
