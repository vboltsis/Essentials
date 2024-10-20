namespace WeatherExample;

public class HelloService : IHelloService
{
    public string SayHello(string name) => $"Hello, {name}!";
}

public interface IHelloService
{
    string SayHello(string name);
}