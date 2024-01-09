namespace XUnitTests;

public interface IInventory
{
    bool IsInStock(string item);
    void ReduceStock(string item);
}

public interface IPaymentGateway
{
    bool ProcessPayment(double amount);
}

public interface IMailService
{
    void SendMail(string message);
}

public class OrderService
{
    private readonly IInventory _inventory;
    private readonly IPaymentGateway _paymentGateway;
    private readonly IMailService _mailService;

    public OrderService(IInventory inventory, IPaymentGateway paymentGateway, IMailService mailService)
    {
        _inventory = inventory;
        _paymentGateway = paymentGateway;
        _mailService = mailService;
    }

    public bool ProcessOrder(string item, double amount)
    {
        if (_inventory.IsInStock(item))
        {
            if (_paymentGateway.ProcessPayment(amount))
            {
                _inventory.ReduceStock(item);
                _mailService.SendMail($"Purchased item {item} for {amount}");
                return true;
            }
        }

        return false;
    }
}
