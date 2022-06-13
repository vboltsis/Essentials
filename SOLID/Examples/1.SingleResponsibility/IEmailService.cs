public interface IEmailService
{
    Task SendEmailAsync(User user, EmailMessage message);
}