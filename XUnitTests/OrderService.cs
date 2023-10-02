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

public class OrderService
{
    private readonly IInventory _inventory;
    private readonly IPaymentGateway _paymentGateway;

    public OrderService(IInventory inventory, IPaymentGateway paymentGateway)
    {
        _inventory = inventory;
        _paymentGateway = paymentGateway;
    }

    public bool ProcessOrder(string item, double amount)
    {
        if (_inventory.IsInStock(item))
        {
            if (_paymentGateway.ProcessPayment(amount))
            {
                _inventory.ReduceStock(item);
                return true;
            }
        }

        return false;
    }
}
