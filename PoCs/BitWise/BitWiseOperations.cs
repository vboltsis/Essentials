using System;
using System.Numerics;

namespace PoCs.BitWise;

public static class BitWiseOperations
{
    public static void TestFruits()
    {
        var bananaApple = Fruits.Banana | Fruits.Apple;

        Console.WriteLine((bananaApple & Fruits.Banana) != 0);
        Console.WriteLine((bananaApple & Fruits.Apple) != 0);
        Console.WriteLine((bananaApple & Fruits.Mango) != 0);

        var removedBanana = bananaApple & (~Fruits.Banana);
        var addedMango = bananaApple ^ Fruits.Mango;

        Console.WriteLine((removedBanana & Fruits.Banana) != 0);
        Console.WriteLine((addedMango & Fruits.Mango) != 0);

        var bananaAppleOrange = bananaApple |= Fruits.Orange;
        Console.WriteLine((bananaAppleOrange & Fruits.Orange) != 0);
        Console.WriteLine((bananaAppleOrange & Fruits.Apple) != 0);

        bananaApple &= Fruits.Banana;

        var isSubset = bananaApple.IsSubsetOf(Fruits.Apple | Fruits.Banana | Fruits.Orange);
        var isSubset2 = bananaApple.IsSubsetOf(Fruits.Banana | Fruits.Orange);
        Console.WriteLine(isSubset);

        var mango = Fruits.Mango;
        mango |= Fruits.Banana;

        Console.ReadLine();
    }

    public static bool CheckIfFruitExists(Fruits fruits, Fruits fruit)
    {
        return (fruits & fruit) != 0;
    }

    public static void AddFruitIfNotExists(Fruits fruits, Fruits fruit)
    {
        fruits |= fruit;
    }

    public static void RemoveFruit(Fruits fruits, Fruits fruit)
    {
        fruits &= ~fruit;
    }

    public static bool IsSubsetOf(this Fruits fruits, Fruits other)
    {
        return other.HasFlag(fruits);
    }
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

