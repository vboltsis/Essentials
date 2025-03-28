namespace FeatureExamples;

public class BuilderPattern
{
    public static void Example()
    {
        var house = new HouseBuilder(4, 10)
                            .WithRoofType("Gable")
                            .WithGarage(true)
                            .Build();

        Console.WriteLine(house);
    }
}

public class House
{
    public int Doors { get; }
    public int Windows { get; }
    public string RoofType { get; }
    public bool Garage { get; }

    public House(int doors, int windows, string roofType, bool garage)
    {
        Doors = doors;
        Windows = windows;
        RoofType = roofType;
        Garage = garage;
    }

    public override string ToString()
    {
        return $"House with {Doors} doors, {Windows} windows, {RoofType} roof, Garage: {Garage}";
    }
}

public class HouseBuilder
{
    private readonly int _doors;
    private readonly int _windows;

    private string _roofType = "Flat";
    private bool _garage = false;

    public HouseBuilder(int doors, int windows)
    {
        _doors = doors;
        _windows = windows;
    }

    public HouseBuilder WithRoofType(string roofType)
    {
        _roofType = roofType;
        return this;
    }

    public HouseBuilder WithGarage(bool garage)
    {
        _garage = garage;
        return this;
    }

    public House Build()
    {
        return new House(_doors, _windows, _roofType, _garage);
    }
}