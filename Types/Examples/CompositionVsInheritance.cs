namespace FeatureExamples;

public abstract class Animal : IEater
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
    public string Type { get; set; }
    
    public override void Eat()
    {
        if (Type == "Bengal")
        {
            Console.WriteLine("Eating as a Bengal");
        }
        else
        {
            base.Eat();
        }
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

public class BengalEater : IEater
{
    public void Eat()
    {
        Console.WriteLine("Eating as a bengal");
    }
}
//---------------------------------------------------------------
//CLASSIC INHERITANCE
public class Shape
{
    public double Area { get; set; }
}

public class Rectangle : Shape
{
    public double Width { get; set; }
    public double Height { get; set; }
}

public class Circle : Shape
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

public class Rectangle1
{
    public AreaProperty AreaInfo { get; set; } = new AreaProperty();
    public WidthHeightProperty Dimensions { get; set; } = new WidthHeightProperty();
}

public class Circle1
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

//Video Game

public class Player
{
    public string Name { get; set; }

    private IAttack _attackBehavior;

    public Player(IAttack attackBehavior)
    {
        _attackBehavior = attackBehavior;
    }

    public void PerformAttack()
    {
        _attackBehavior.Attack();
    }

    public void ChangeAttackBehavior(IAttack behavior)
    {
        _attackBehavior = behavior;
    }
}

public interface IAttack
{
    void Attack();
}
