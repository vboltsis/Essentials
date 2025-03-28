namespace FeatureExamples;

public abstract class Animal1
{
    public virtual void Move()
    {
        Console.WriteLine("The animal moves.");
    }

    public virtual void MakeSound()
    {
        Console.WriteLine("The animal makes a sound.");
    }
}

public class Dog : Animal1
{
    public override void Move()
    {
        Console.WriteLine("The dog runs.");
    }

    public override void MakeSound()
    {
        Console.WriteLine("The dog barks.");
    }
}

public class Bird : Animal1
{
    public override void Move()
    {
        Console.WriteLine("The bird flies.");
    }

    public override void MakeSound()
    {
        Console.WriteLine("The bird chirps.");
    }
}

public class Fish : Animal1
{
    public override void Move()
    {
        Console.WriteLine("The fish swims.");
    }

    public override void MakeSound()
    {
        Console.WriteLine("The fish bubbles.");
    }
}


public interface IMoveBehavior
{
    void Move();
}

public interface ISoundBehavior
{
    void MakeSound();
}

// Movement Behaviors
public class RunBehavior : IMoveBehavior
{
    public void Move()
    {
        Console.WriteLine("Runs swiftly.");
    }
}

public class FlyBehavior : IMoveBehavior
{
    public void Move()
    {
        Console.WriteLine("Flies gracefully.");
    }
}

public class SwimBehavior : IMoveBehavior
{
    public void Move()
    {
        Console.WriteLine("Swims smoothly.");
    }
}

// Sound Behaviors
public class BarkBehavior : ISoundBehavior
{
    public void MakeSound()
    {
        Console.WriteLine("Barks loudly.");
    }
}

public class ChirpBehavior : ISoundBehavior
{
    public void MakeSound()
    {
        Console.WriteLine("Chirps melodiously.");
    }
}

public class BubbleBehavior : ISoundBehavior
{
    public void MakeSound()
    {
        Console.WriteLine("Makes bubbling sounds.");
    }
}

public class Animal
{
    private IMoveBehavior _moveBehavior;
    private ISoundBehavior _soundBehavior;

    public Animal(IMoveBehavior moveBehavior, ISoundBehavior soundBehavior)
    {
        _moveBehavior = moveBehavior;
        _soundBehavior = soundBehavior;
    }

    public void PerformMove()
    {
        _moveBehavior.Move();
    }

    public void PerformSound()
    {
        _soundBehavior.MakeSound();
    }

    // Methods to change behaviors at runtime
    public void SetMoveBehavior(IMoveBehavior moveBehavior)
    {
        _moveBehavior = moveBehavior;
    }

    public void SetSoundBehavior(ISoundBehavior soundBehavior)
    {
        _soundBehavior = soundBehavior;
    }
}


