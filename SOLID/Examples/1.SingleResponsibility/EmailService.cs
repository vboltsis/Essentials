public record EmailService : IEmailService
{
    public Task SendEmailAsync(User user, EmailMessage message)
    {
        // Send email
        return null;
    }
}
