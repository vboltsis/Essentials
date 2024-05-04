namespace WeatherExample;

public class EmailNotificationService : INotificationService
{
    public string Notify(string message) => $"[Email] {message}";
}
