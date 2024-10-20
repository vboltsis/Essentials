using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

string connectionString = "Server=.;Database=OrleansDemo;Trusted_Connection=True;TrustServerCertificate=True";

using (var connection = new SqlConnection(connectionString))
{
    connection.Open();

    string sql = "SELECT Id, FirstName, LastName FROM Users";
    var users = (await connection.QueryAsync<User>(sql)).ToList();

    Console.WriteLine("Users in the database:");
    foreach (var user in users)
    {
        Console.WriteLine($"{user.FirstName} {user.LastName}");
    }

    // INSERT
    var newUser = new User { FirstName = "New", LastName = "User" };
    string insertSql = "INSERT INTO Users (FirstName, LastName) VALUES (@FirstName, @LastName)";
    var affectedRows = await connection.ExecuteAsync(insertSql, newUser);
    Console.WriteLine($"Inserted {affectedRows} new user(s).");

    // UPDATE
    var userToUpdate = new User { Id = 2, FirstName = "DomePlane" }; // Assuming Id=2 exists
    string updateSql = "UPDATE Users SET FirstName = @FirstName WHERE Id = @Id";
    affectedRows = await connection.ExecuteAsync(updateSql, userToUpdate);
    Console.WriteLine($"Updated {affectedRows} user(s).");

    // DELETE
    string deleteSql = "DELETE FROM Users WHERE Id = @Id"; // Assuming Id=1 exists
    affectedRows = await connection.ExecuteAsync(deleteSql, new { Id = 1 });
    Console.WriteLine($"Deleted {affectedRows} user(s).");
}

using (var connection = new SqlConnection(connectionString))
{
    connection.Open();

    using (var multi = await connection.QueryMultipleAsync("sp_GetUserAndOrders", new { UserId = 3 }, commandType: CommandType.StoredProcedure))
    {
        var user = (await multi.ReadAsync<User>()).FirstOrDefault();

        var orders = (await multi.ReadAsync<Order>()).ToList();

        Console.WriteLine($"User: {user.FirstName} {user.LastName}");

        Console.WriteLine("Orders:");
        foreach (var order in orders)
        {
            Console.WriteLine($"Order Date: {order.OrderDate}, Amount: {order.Amount}");
        }
    }
}

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal Amount { get; set; }
}