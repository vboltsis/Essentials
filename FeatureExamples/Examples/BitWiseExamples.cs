﻿namespace FeatureExamples;

public static class BitWiseExamples
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

/* TypeScript equivalent
enum Fruits {
    None = 1 << 0,
    Banana = 1 << 1, //2
    Apple = 1 << 2, //4
    Orange = 1 << 3, //8
    PineApple = 1 << 4, //16
    WaterMelon = 1 << 5, //32
    Melon = 1 << 6, //64
    Mango = 1 << 7, //128
    Apricot = 1 << 8 //256
}

var bananaApple = Fruits.Banana | Fruits.Apple;

console.log((bananaApple & Fruits.Banana) != 0);
console.log((bananaApple & Fruits.Apple) != 0);
console.log((bananaApple & Fruits.Mango) != 0);

var removedBanana = bananaApple & (~Fruits.Banana);
var addedMango = bananaApple ^ Fruits.Mango;

console.log((removedBanana & Fruits.Banana) != 0);
console.log((addedMango & Fruits.Mango) != 0);

var bananaAppleOrange = bananaApple |= Fruits.Orange;
console.log((bananaAppleOrange & Fruits.Orange) != 0);
console.log((bananaAppleOrange & Fruits.Apple) != 0);

bananaApple &= Fruits.Banana;
*/

/* blog post code
 UserFlags flags = UserFlags.HasSubscription;

// Set flags
flags |= UserFlags.IsAdmin;
flags |= UserFlags.IsVerified;

// Check if IsAdmin is set
bool isAdmin = (flags & UserFlags.IsAdmin) != 0;

Console.WriteLine(isAdmin);

// Unset a flag
flags &= ~UserFlags.IsAdmin;

Console.WriteLine(isAdmin);
Console.WriteLine(flags);

[Flags]
public enum UserFlags : byte
{
    IsAdmin = 1 << 0,
    IsVerified = 1 << 1,
    HasSubscription = 1 << 2,
    IsOnTrial = 1 << 3,
    IsBanned = 1 << 4,
    IsDeleted = 1 << 5,
    IsEmailConfirmed = 1 << 6,
    IsPhoneConfirmed = 1 << 7
}

 
 */