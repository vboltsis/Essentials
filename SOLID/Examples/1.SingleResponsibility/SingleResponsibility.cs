#region Definition
/*
The Single Responsibility Principle (SRP)
The idea behind the SRP is that every class, module, or function in a program
should have one responsibility/purpose in a program.
As a commonly used definition, "every class should have only one reason to change".
 */
#endregion

public record SingleResponsibility
{
    private readonly IEmailService _emailService;
    private readonly IUserService _userService;

    public SingleResponsibility(IEmailService emailService, IUserService userService)
    {
        _emailService = emailService;
        _userService = userService;
    }

    public async Task RegisterUser(User user)
    {
        //Imagine some code to validate user Or
        //a Middleware would validate him before this method
        await _userService.CreateUserAsync(user);

        await _emailService.SendEmailAsync(user, new EmailMessage
        {
            Body = "Lorem",
            Subject = "Ipsum"
        });
    }
    
    //INCORRECT WAY
    /*
    public async Task RegisterUser(User user)
    {
        if (user.FirstName == null)
        {
            throw new Exception("FirstName cannot be null");
        }

        if (user.LastName == null)
        {
            throw new Exception("LastName cannot be null");
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        await SendEmailAsync(user, new EmailMessage
        {
            Subject = "",
            Body = ""
        });
    }
    
    public async Task SendEmailAsync(User user, EmailMessage message)
    {
        return null;
    }
    */
}
