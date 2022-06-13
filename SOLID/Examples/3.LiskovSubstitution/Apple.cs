/*
Child classes must do whatever their parents can.
If a parent class can deliver coffee,
then the child class should deliver a capppucino and not water.
This principle aims to enforce consistency so that the parent Class or
its child Class can be used in the same way without any errors.
 */
public record Apple : Fruit
{
    public override string GetColor()
    {
        return "Red color";
    }
}
