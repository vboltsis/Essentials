/*
Clients should not be forced to depend on methods that they do not use. 
When a Class is required to perform actions that are not useful,
it is wasteful and may produce unexpected bugs if the Class
does not have the ability to perform those actions.
A Class should perform only actions that are needed to fulfil its role.
Any other action should be removed completely or moved somewhere else
if it might be used by another Class in the future.
 */
public record InterfaceSegregation
{
    public static void CorrectInheritance()
    {
        //Jeans inherit the correct interfaces and so does cap
        var jeans = new Jeans();
        var cap = new BaseballCap();
    }
}
