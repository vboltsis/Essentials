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