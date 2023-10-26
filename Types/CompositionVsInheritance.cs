namespace FeatureExamples;

public class Animal : IEater
{
    public string Name { get; set; }

    public virtual void Eat()
    {
        Console.WriteLine("Eating");
    }
}

//CLASSIC INHERITANCE
public class Cat1 : Animal
{
    public override void Eat()
    {
        Console.WriteLine("Eat as a cat");
    }
}

public class Dog1 : Animal
{
    public override void Eat()
    {
        Console.WriteLine("Eat as a dog");
    }
}

//COMPOSITION
public interface IEater
{
    void Eat();
}

public class Cat
{
    public string Claws { get; set; }

    private readonly IEater _eater;

    public Cat(IEater eater)
    {
        _eater = eater;
    }

    public void Eat()
    {
        _eater.Eat();
    }
}

public class CatEater : IEater
{
    public void Eat()
    {
        Console.WriteLine("Eat as a cat");
    }
}

//---------------------------------------------------------------
//CLASSIC INHERITANCE
public class Shape1
{
    public double Area { get; set; }
}

public class Rectangle : Shape1
{
    public double Width { get; set; }
    public double Height { get; set; }
}

public class Circle : Shape1
{
    public double Radius { get; set; }
}


//COMPOSITION
public class AreaProperty
{
    public double Area { get; set; }
}

public class WidthHeightProperty
{
    public double Width { get; set; }
    public double Height { get; set; }
}

public class RadiusProperty
{
    public double Radius { get; set; }
}

public class Rectangle
{
    public AreaProperty AreaInfo { get; set; } = new AreaProperty();
    public WidthHeightProperty Dimensions { get; set; } = new WidthHeightProperty();
}

public class Circle
{
    public AreaProperty AreaInfo { get; set; } = new AreaProperty();
    public RadiusProperty RadiusInfo { get; set; } = new RadiusProperty();
}

public class SquareCircleCombo
{
    public AreaProperty AreaInfo { get; set; } = new AreaProperty();
    public WidthHeightProperty Dimensions { get; set; } = new WidthHeightProperty();
    public RadiusProperty RadiusInfo { get; set; } = new RadiusProperty();
}