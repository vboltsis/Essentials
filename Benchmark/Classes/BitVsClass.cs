namespace Benchmark;

/*
|                  Method |       Mean |     Error |    StdDev |   Gen0 | Allocated |
|------------------------ |-----------:|----------:|----------:|-------:|----------:|
| CheckIfFruitExistsClass | 25.9151 ns | 0.2644 ns | 0.2473 ns | 0.0019 |      24 B |
|   CheckIfFruitExistsBit |  0.0320 ns | 0.0105 ns | 0.0093 ns |      - |         - | 
*/
[MemoryDiagnoser]
public class BitVsClass
{
    private HashSet<Fruit> _fruits;
    private Fruits _fruitsBit;

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

        _fruitsBit = Fruits.Tomato | Fruits.Banana | Fruits.Apple | Fruits.Orange | Fruits.PineApple |
            Fruits.WaterMelon | Fruits.Melon | Fruits.Mango | Fruits.Apricot;
    }

    [Benchmark]
    public bool CheckIfFruitExistsClass()
    {
        return _fruits.Contains(new Fruit { Name = "Mango" });
    }

    [Benchmark]
    public bool CheckIfFruitExistsBit()
    {
        return (_fruitsBit & Fruits.Mango) != 0;
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
