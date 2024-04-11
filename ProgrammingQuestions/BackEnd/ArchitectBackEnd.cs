/* 1. What is the CAP theorem
The CAP Theorem, also known as Brewer's Theorem,
is a fundamental principle in the field of distributed computing and database systems.
Formulated by Eric Brewer in 2000, the theorem states that in any distributed data store,
only two out of the following three guarantees can be achieved simultaneously:

1. Consistency
Definition: Every read receives the most recent write or an error. In other words, all nodes see the same data at the same time. Consistency here refers to the data being uniform across all nodes in the system.
Implication: If a data item is updated, that update must be immediately visible to all subsequent transactions, regardless of which node in the cluster they access.

2. Availability
Definition: Every request receives a (non-error) response, without the guarantee that it contains the most recent write. It means the system remains operational and accessible (read and write operations) at all times.
Implication: Any request made to the system must return a response, even if one or more nodes are down or some data is not up-to-date.

3. Partition Tolerance
Definition: The system continues to operate despite network failures that prevent communication between nodes in the system. Partition tolerance is essential in any distributed system as network partitions are unavoidable.
Implication: The system can continue to operate even if there is a loss of message(s) between nodes due to network failures.

~Understanding CAP in Practice
Trade-offs: In reality, partition tolerance is non-negotiable in distributed systems,
as network failures are a fact of life. Therefore, the real trade-off is between consistency and availability.

~Consistency vs Availability:
-A system that chooses consistency over availability will ensure that all nodes have the most recent data
but might refuse to respond to read/write operations during a network partition to maintain consistency.

-A system that chooses availability over consistency will always process queries and tries to return the most recent available version of the data,
even if some nodes are partitioned. However, this might mean that not all nodes have the latest data immediately.

Consistency and Partition Tolerance (CP)
Systems that prioritize Consistency and Partition Tolerance may compromise Availability during network partitions. Examples include:

HBase: A distributed, scalable, big data store, part of the Apache Hadoop ecosystem, providing consistent reads and writes.
MongoDB: In certain configurations, particularly with its replica sets, MongoDB can be CP, ensuring consistency and partition tolerance.
Zookeeper: Primarily used for coordination and configuration management in distributed systems, Zookeeper ensures consistent state across all nodes.
Redis (with Redis Cluster configuration): While Redis is often used as an in-memory data store with a focus on performance, in cluster mode it can provide CP characteristics.
CockroachDB: A SQL database that provides strong consistency and partition tolerance, often used in financial and other transactional systems.

Availability and Partition Tolerance (AP)
Systems that prioritize Availability and Partition Tolerance may allow for some inconsistency (eventual consistency) during network partitions. Examples include:

Cassandra: Known for its high availability and partition tolerance, Cassandra offers eventual consistency across its distributed architecture.
CouchDB: A document-oriented NoSQL database that prioritizes high availability and partition tolerance, using eventual consistency for data replication.
DynamoDB: Amazon's NoSQL database is designed for high availability and partition tolerance, with eventual consistency options.
Riak: A distributed NoSQL database designed for high availability, fault tolerance, and operational simplicity, providing eventual consistency.
Couchbase: Initially based on CouchDB, Couchbase Server is designed for high throughput and scalability, offering AP characteristics.

Consistency and Availability (CA)
Systems that prioritize Consistency and Availability typically do not tolerate network partitions well. These are often traditional, single-node databases. Examples include:

MySQL: A relational database management system that, in a single-node setup, focuses on consistency and availability.
PostgreSQL: Similar to MySQL, PostgreSQL in a single-node setup provides consistency and availability but struggles with partition tolerance.
Microsoft SQL Server: In a non-distributed setup, SQL Server ensures data consistency and high availability.
Oracle Database: Oracle RDBMS, particularly with a single-instance configuration, focuses on consistency and availability.
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

