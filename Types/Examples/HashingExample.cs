using System.Security.Cryptography;
using System.Text;

namespace FeatureExamples;

public class HashingExample
{
    private const string Pepper = "MySaltNPeppa";

    private static string GenerateSalt()
    {
        byte[] saltBytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }

        return Convert.ToBase64String(saltBytes);
    }

    private static string HashPassword(string password, string salt)
    {
        using (var sha256 = SHA256.Create())
        {
            var saltedPassword = password + salt;
            byte[] saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
            byte[] hashBytes = sha256.ComputeHash(saltedPasswordBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }

    private static string ApplyPepper(string hashedPassword)
    {
        using (var sha256 = SHA256.Create())
        {
            var pepperedPassword = hashedPassword + Pepper;
            byte[] pepperedPasswordBytes = Encoding.UTF8.GetBytes(pepperedPassword);
            byte[] hashBytes = sha256.ComputeHash(pepperedPasswordBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }

    public static User RegisterUser(string password)
    {
        string salt = GenerateSalt();
        string saltedHashedPassword = HashPassword(password, salt);

        return new User { Salt = salt, HashedPassword = saltedHashedPassword };
    }

    public static bool AuthenticateUser(string enteredPassword, User user)
    {
        string saltedHashedPassword = HashPassword(enteredPassword, user.Salt);
        return saltedHashedPassword == user.HashedPassword;
    }

    public static void Example()
    {
        string password = "UserPassword123";
        User newUser = RegisterUser(password);

        bool isAuthenticated = AuthenticateUser("UserPassword123", newUser);
        Console.WriteLine("User authenticated: " + isAuthenticated);
    }
}

public class User
{
    public string Name { get; set; }
    public string Salt { get; set; }
    public string HashedPassword { get; set; }
}
