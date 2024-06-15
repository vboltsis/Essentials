using System.Security.Cryptography;

namespace FeatureExamples;

public class DecoratorPattern
{
    public void StreamExample()
    {
        string filePath = "encryptedData.txt";
        string originalMessage = "Hello, world! This is a test message.";

        using (Aes aes = Aes.Create())
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
            using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateEncryptor(aes.Key, aes.IV), CryptoStreamMode.Write))
            using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
            {
                streamWriter.Write(originalMessage);
            }

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateDecryptor(aes.Key, aes.IV), CryptoStreamMode.Read))
            using (StreamReader streamReader = new StreamReader(cryptoStream))
            {
                string decryptedMessage = streamReader.ReadToEnd();
                Console.WriteLine($"Decrypted message: {decryptedMessage}");
            }
        }
    }

    public static void CoffeExample()
    {
        var coffee = new Coffee();
        Console.WriteLine(coffee.GetCost()); // 5
        Console.WriteLine(coffee.GetDescription()); // Coffee

        var milkCoffee = new MilkDecorator(coffee);
        Console.WriteLine(milkCoffee.GetCost()); // 7
        Console.WriteLine(milkCoffee.GetDescription()); // Coffee, Milk

        var sugarCoffee = new SugarDecorator(milkCoffee);
        Console.WriteLine(sugarCoffee.GetCost()); // 8
        Console.WriteLine(sugarCoffee.GetDescription()); // Coffee, Milk, Sugar
    }

    public interface ICoffee
    {
        int GetCost();
        string GetDescription();
    }

    public class Coffee : ICoffee
    {
        public int GetCost()
        {
            return 5;
        }

        public string GetDescription()
        {
            return "Coffee";
        }
    }

    public class CoffeeDecorator : ICoffee
    {
        private readonly ICoffee _coffee;

        public CoffeeDecorator(ICoffee coffee)
        {
            _coffee = coffee;
        }

        public virtual int GetCost()
        {
            return _coffee.GetCost();
        }

        public virtual string GetDescription()
        {
            return _coffee.GetDescription();
        }
    }

    public class MilkDecorator : CoffeeDecorator
    {
        public MilkDecorator(ICoffee coffee) : base(coffee)
        {
        }

        public override int GetCost()
        {
            return base.GetCost() + 2;
        }

        public override string GetDescription()
        {
            return $"{base.GetDescription()}, Milk";
        }
    }

    public class SugarDecorator : CoffeeDecorator
    {
        public SugarDecorator(ICoffee coffee) : base(coffee)
        {
        }

        public override int GetCost()
        {
            return base.GetCost() + 1;
        }

        public override string GetDescription()
        {
            return $"{base.GetDescription()}, Sugar";
        }
    }
}
