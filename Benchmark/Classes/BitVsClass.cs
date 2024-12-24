namespace Benchmark;

/*
| Method                  | Mean      | Error     | StdDev    | Allocated |
|------------------------ |----------:|----------:|----------:|----------:|
| CheckIfFruitExistsClass | 15.857 ns | 0.2194 ns | 0.2052 ns |         - |
| CheckIfFruitExistsBit   |  2.902 ns | 0.0039 ns | 0.0035 ns |         - |
*/
[MemoryDiagnoser] 
[ReturnValueValidator(true)]
public class BitVsClass
{
    private HashSet<Fruit> _fruits;
    private HashSet<Fruits> _fruitsBit;

    private static Fruit _fruit = new() { Name = "Mango" };
    private static Fruits _fruitBit = Fruits.Mango;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _fruits = new HashSet<Fruit>
        {
            new Fruit { Name = "Tomato" },
            new Fruit { Name = "Banana" },
            new Fruit { Name = "Apple" },
            new Fruit { Name = "Orange" },
            new Fruit { Name = "PineApple" },
            new Fruit { Name = "WaterMelon" },
            new Fruit { Name = "Melon" },
            new Fruit { Name = "Mango" },
            new Fruit { Name = "Apricot" }
        };

        _fruitsBit = new HashSet<Fruits>
        { 
            Fruits.Tomato,
            Fruits.Banana,
            Fruits.Apple,
            Fruits.Orange,
            Fruits.PineApple,
            Fruits.WaterMelon,
            Fruits.Melon,
            Fruits.Mango,
            Fruits.Apricot
        };
    }

    [Benchmark]
    public bool CheckIfFruitExistsClass()
    {
        return _fruits.Contains(_fruit);
    }

    [Benchmark]
    public bool CheckIfFruitExistsBit()
    {
        return _fruitsBit.Contains(_fruitBit);
    }
}

record Fruit
{
    public string Name { get; set; }
}

[Flags]
public enum Fruits : long
{
    Tomato = 1 << 0,
    Banana = 1 << 1, //2
    Apple = 1 << 2, //4
    Orange = 1 << 3, //8
    PineApple = 1 << 4, //16
    WaterMelon = 1 << 5, //32
    Melon = 1 << 6, //64
    Mango = 1 << 7, //128
    Apricot = 1 << 8 //256
}
