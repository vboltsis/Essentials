#region Definition
/*
 Robert C. Martin considered this principle as
“the most important principle of object-oriented design”.
But he wasn’t the first one who defined it.
Bertrand Meyer wrote about it in 1988 in his book Object-Oriented Software Construction.
He explained the Open/Closed Principle as:
“Software entities (classes, modules, functions, etc.) should be open for extension,
but closed for modification.”

It tells you to write your code so that you will be able to add new functionality,
without changing the existing code.
That prevents situations in which a change to one of your classes,
also requires you to adapt all depending classes.
Unfortunately, Bertrand Mayer proposes to use inheritance to achieve this goal:

“A class is closed, since it may be compiled, stored in a library,
baselined, and used by client classes. But it is also open,
since any new class may use it as parent, adding new features.
When a descendant class is defined, there is no need to change the original or
to disturb its clients.”

But as we’ve learned over the years and as other authors explained in great details,
e.g., Robert C. Martin in his articles about the SOLID principles or
Joshua Bloch in his book Effective Java, inheritance introduces tight coupling
if the subclasses depend on implementation details of their parent class.

That’s why Robert C. Martin and others redefined the Open/Closed Principle to the
Polymorphic Open/Closed Principle.
It uses interfaces instead of superclasses to allow different implementations
which you can easily substitute without changing the code that uses them.
The interfaces are closed for modifications, and you can provide new implementations
to extend the functionality of your software.

The main benefit of this approach is that an interface,
introduces an additional level of abstraction which enables loose coupling.
The implementations of an interface are independent of each other and
don’t need to share any code.
If you consider it beneficial that two implementations of an interface share some code,
you can either use inheritance or composition.
 */
#endregion
public interface IShape
{
    double Area { get; set; }
    double CalculateArea();    
}
