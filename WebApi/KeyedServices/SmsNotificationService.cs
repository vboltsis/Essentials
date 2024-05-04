namespace WeatherExample;

public class SmsNotificationService : INotificationService
{
    public string Notify(string message) => $"[SMS] {message}";
}
