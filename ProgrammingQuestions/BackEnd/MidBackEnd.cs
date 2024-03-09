/*4. What is a Garbage Collector
reference: https://docs.microsoft.com/en-us/dotnet/standard/garbage-collection/fundamentals
In the common language runtime (CLR), the garbage collector (GC) serves as an automatic memory manager.
The garbage collector manages the allocation and release of memory for an application.
For developers working with managed code, this means that you don't have to write code to perform memory management tasks.
Automatic memory management can eliminate common problems,
such as forgetting to free an object and causing a memory leak or attempting to access memory for an object that's already been freed.
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

/* 9. out vs ref keyword
out allows methods to return more than one thing. This is useful when you want to return a value and a reference to a variable.
ref allows methods to return a reference to a variable. This is useful when you want to return a reference to a variable.
 */

/*11. What is an idempotent operation
 No matter how many times you call the operation, the result will be the same.
*/

/* 32. What is RabbitMQ
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
-Dead Letter Queue: When a message can't be processed or delivered, it can be sent to a dead letter queue.
-Clustering and High Availability: RabbitMQ can be run in a cluster to ensure high availability and reliability.
-Federation and Shovel: These are mechanisms to connect multiple brokers (or clusters) to share messages between them.
 
 */