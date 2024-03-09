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

/* 10. const vs readonly
const are used to declare variables that are immutable.
This means that the value of the variable cannot change after it is initialized.
readonly are used to declare variables that are read-only.
The difference is that const is initialized at compile time while readonly is initialized at runtime
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
and is used to store a variety of data structures,
including the module table, type table, method table, and static field data.

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
In .NET, the terms "managed stack" and "unmanaged stack"
refer to two different types of stacks that are used by the runtime.

Managed Stack:
The managed stack is a region of memory used by the .NET runtime
to store information related to the execution of managed code.
The managed stack is used to store method parameters, local variables,
and other information related to the execution of managed code.
When a method is called, a new frame is created on the managed stack to store information
related to that method's execution.

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

/* 27. What is the difference between a thread and a task in .NET
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

/* 28. How many types of design patterns exist and give examples of some of them
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

/* 29. What are Sealed classes and how to extend a sealed class
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
    private MySealedClass _mySealedClass;
    
    public MyAggregatedClass(MySealedClass mySealedClass)
    {
        _mySealedClass = mySealedClass;
    }
    
    // Additional methods and properties
}
*/

/* 30. What is the difference between virtual and abstract methods in C#

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

/* 31. What are Immutable Types in C#
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

//Cool links
//https://endjin.com/blog/2020/09/arraypool-vs-memorypool-minimizing-allocations-ais-dotnet
//https://github.com/Maoni0/mem-doc/blob/master/doc/.NETMemoryPerformanceAnalysis.md
//https://www.youtube.com/watch?v=Z8M2BTscoD4
Console.WriteLine("Hello Learner! I hope this solution helps you become better!");
