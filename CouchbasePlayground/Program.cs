using Couchbase;

//docker pull couchbase
//docker run -d --name my-couchbase -p 8091-8094:8091-8094 -p 11210:11210 couchbase

var cluster = await Cluster.ConnectAsync("couchbase://localhost", "Administrator", "password");
var bucket = await cluster.BucketAsync("MyBucket");
var collection = bucket.DefaultCollection();

var dummyData = new User
{
    Id = 2,
    Name = "John Doe",
    Email = "john.doe2@example.com"
};

try
{
    //await collection.UpsertAsync(dummyData.Id.ToString(), dummyData);
    //Console.WriteLine("Document inserted successfully.");

    var query = "SELECT b.* FROM `MyBucket` b WHERE b.`name` = 'John Doe';";
    var result = await cluster.QueryAsync<User>(query);

    await foreach (var row in result.Rows)
    {
        Console.WriteLine($"Name: {row.Name}, Email: {row.Email}");
    }
    //var getResult = await collection.GetAsync("1");
    //var user = getResult.ContentAs<User>();
    //Console.WriteLine($"Name: {user?.Name}, Email: {user?.Email}");

}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

// Always good practice to disconnect when done
await cluster.DisposeAsync();

Console.WriteLine("Press any key to exit...");

Console.ReadKey();
 
record User
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
}