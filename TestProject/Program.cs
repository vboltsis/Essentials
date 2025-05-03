Evil evil = new Evil() { value = 2 };
Foo(evil);
Console.WriteLine(evil.value);

static void Foo<T>(T mutable)
    where T : IMutable
{
    mutable.Mutate();
    Console.WriteLine(mutable.Value);
}

interface IMutable
{
    void Mutate();
    int Value { get; }
}

struct Evil : IMutable
{
    public int value;

    public void Mutate()
    {
        value = 9;
    }

    public int Value { get { return value; } }
}
