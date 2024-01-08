/* 1. What are the different data types in JavaScript?
 * 
JavaScript primarily has two types of data types: Primitive and Non-primitive.

~Primitive Types: These are the basic data types that include:

String: Represents textual data, e.g., "Hello, World!".
Number: Represents both integer and floating-point numbers, e.g., 42, 3.14.
Boolean: Represents a logical entity and can have two values: true or false.
Undefined: Represents a variable that has been declared but not assigned a value.
Null: Represents an intentional absence of any value. It's typically used to signify 'no value'.
Symbol: Introduced in ES6, symbols are immutable and unique and can be used as object keys.
BigInt: Also introduced in ES6, represents integers with arbitrary precision, useful for very large numbers.

~Non-primitive Types: These include:

1. Object
The most fundamental non-primitive type in JavaScript is the Object.
It's a collection of properties, where each property is a key-value pair.
Properties can be of any type, including other objects, which allows for a nested structure.
Objects in JavaScript are highly versatile and can represent complex entities, like user profiles,
product details, etc.

2. Array
An array is a special kind of object that represents a collection of elements.
These elements can be of any type, including other arrays and objects.
Arrays have a set of special methods for iteration and manipulation, like push(), pop(), map(), and filter().

3. Function
Functions are also objects in JavaScript.
Beyond just being callable (i.e., you can run the function),
functions can have properties and methods just like other objects.
They are first-class citizens in JavaScript, meaning they can be stored in variables,
passed as arguments to other functions, returned from functions, and possess properties and methods.

4. Date
It is used to work with dates and times.
You can create a new date object using new Date().
This object has methods to get and set the year, month, day, hour, minute, second, and millisecond,
as well as to manipulate and format dates.

5. RegExp
Regular expressions are used for string searching and manipulation.
They are objects of the RegExp type and provide methods for text searching and pattern matching.

6. Error
The Error object is used for handling errors.
It can capture error information like a message string and a call stack.
There are also several built-in error types in JavaScript, such as SyntaxError, TypeError, RangeError, etc.
 */

/* 2. What is the difference between == and === operators? 
The == operator compares only the values of the variables,
whereas === compares both the values and types of the variables.
*/

/* 3. Can you explain the difference between let, const, and var?
~var:

Scope: Function-scoped, meaning it is only available within the function where it was defined.
Hoisting: Variables declared with var are hoisted, meaning they can be accessed before their declaration
(though they will be undefined until their definition line is executed).
Re-declaration: Allows re-declaration and updating of the variable within its scope.

~let:

Scope: Block-scoped, making it only available within the block (like loops or if-statements)
where it was defined.

Hoisting: Variables declared with let are also hoisted, but unlike var,
accessing them before the declaration results in a ReferenceError.

Re-declaration: Does not allow re-declaration in the same scope, but allows the value to be updated.

~const:

Scope: Block-scoped, similar to let.

Hoisting: Behaves like let in terms of hoisting.

Re-declaration: Does not allow re-declaration or updating.

It’s important to note that for objects and arrays,
const only prevents reassignment of the variable identifier, not the modification of the object/array itself.
*/

/* 4. What is the difference between null and undefined?
In JavaScript, null and undefined are both primitive values that represent absence of value, but they are used in slightly different ways:

Undefined:

undefined means a variable has been declared but has not yet been assigned a value.
It is the default value of variables that are declared but not initialized.
It's also the value returned by a function that doesn't explicitly return anything.
When you try to access a property of an object that doesn't exist, you get undefined.
Null:

null is an assignment value that represents a deliberate non-value (and is a primitive value).
It is used to indicate a deliberate absence of any object value.
It is often used where an object is expected but is not applicable.
When you want to intentionally clear the value of a variable, you can set it to null.
In summary, undefined typically indicates that a variable has not been initialized or
a property does not exist, while null is used as an intentional absence of any object value.
In terms of comparison, undefined and null are equal in loose equality (undefined == null is true),
but not in strict equality (undefined === null is false).
*/

/* 5. How does JavaScript handle asynchronous operations? Can you give an example of a callback function?
JavaScript handles asynchronous operations using mechanisms like Callbacks, Promises, and Async/Await.

~Callbacks:

A callback is a function passed into another function as an argument, which is then invoked inside the outer function to complete some kind of routine or action.
Initially, callbacks were the primary method for asynchronous operations in JavaScript.
While effective, they can lead to complex code structures known as "callback hell" when dealing with multiple asynchronous operations.

~Promises:

A Promise is an object representing the eventual completion or failure of an asynchronous operation.
It allows you to associate handlers with an asynchronous action's eventual success value or failure reason.
Promises help write cleaner, more readable asynchronous code, avoiding the callback hell.
They can be chained, which means the output of one promise can be used as input for another.

~Async/Await:

Introduced in ES2017, async/await is syntactic sugar built on top of promises.
It allows writing asynchronous code in a more synchronous fashion.
An async function returns a promise implicitly, and the await keyword is used to wait for a promise to resolve.
It makes the code look cleaner and more readable, especially for chaining multiple asynchronous calls.

Each of these methods has its own use cases and advantages.
Callbacks are simple and straightforward for basic asynchronous operations.
Promises are more powerful and flexible, especially for complex operations involving chaining or error handling.
Async/await further simplifies the syntax and readability, making asynchronous code look more like traditional synchronous code.
The underlying mechanism of the event loop and Web APIs ensures non-blocking behavior, which is a core feature of JavaScript's asynchronous nature.
*/