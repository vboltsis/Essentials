namespace Benchmark;

/*
| Method                  | Mean      | Error     | StdDev    | Gen0   | Allocated |
|------------------------ |----------:|----------:|----------:|-------:|----------:|
| CheckIfFruitExistsClass | 32.465 ns | 0.1069 ns | 0.1000 ns | 0.0029 |      24 B |
| CheckIfFruitExistsBit   |  3.523 ns | 0.0966 ns | 0.1074 ns |      - |         - |
*/
[MemoryDiagnoser]
public class BitVsClass
{
    private HashSet<Fruit> _fruits;
    private HashSet<Fruits> _fruitsBit;

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
        return _fruits.Contains(new Fruit { Name = "Mango" });
    }

    [Benchmark]
    public bool CheckIfFruitExistsBit()
    {
        return _fruitsBit.Contains(Fruits.Apple);
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
