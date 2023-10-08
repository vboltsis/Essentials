using Newtonsoft.Json;
using StackExchange.Redis;

//FOR PORT 10001 USE THE COMMAND BELOW
//docker run -p 10001:6379 -p 13333:8001 redis/redis-stack:latest

//default redis port is 6379
var conn = ConnectionMultiplexer.Connect("localhost:6379");
//getting database instance of the redis  
IDatabase database = conn.GetDatabase();

//await database.HashSetAsync("test1", new HashEntry[]
//{
//    new HashEntry(55, 77),
//    new HashEntry(33, 11),
//    new HashEntry(22, 99),
//});

var fiftyFive = long.TryParse(database.HashGet("test1", 55), out var version);

var test = new RedisClass { Name = "Moschato", Age = 420 };
await database.StringSetAsync("test", JsonConvert.SerializeObject(test));

//remove key
await database.KeyDeleteAsync("test");

var test2 = await database.StringGetAsync("test");
var boo = test2.IsNull;

var what = JsonConvert.DeserializeObject<RedisClass>(test2);


await database.SetAddAsync("test1", 420);
await database.SetAddAsync("test1", 69);
//var result = await database.KeyExpireAsync("test1", DateTime.Now.AddSeconds(5));

var members = await database.SetMembersAsync("test1");
await database.KeyDeleteAsync("test1");
members = await database.SetMembersAsync("test1");


//var watch = new Stopwatch();
//watch.Start();
//List<Task<bool>> list = new(1_000_000);

//for (int i = 0; i < 1_000_000; i++)
//{
//    list.Add(database.SetAddAsync("test", i));
//}

//Task.WhenAll(list);

//watch.Stop();
//Console.WriteLine(watch.Elapsed);
//Console.WriteLine(watch.ElapsedMilliseconds);
//await database.SetAddAsync("test", 1);
//await database.SetAddAsync("test", 2);


var what3 = database.SetContains("test", 900_000);
Console.WriteLine();


/*set value in redis server  
database.StringSet("redisKey", "redisvalue");
//get value from redis server  
var value = database.StringGet("redisKey");
Console.WriteLine("Value cached in redis server is: " + value);
Console.ReadLine();*/

class RedisClass
{
    public string Name { get; set; }
    public int Age { get; set; }
}