using NSubstitute;

namespace XUnitTests;

public class OrderServiceTests
{
    private readonly IInventory _inventory;
    private readonly IPaymentGateway _paymentGateway;
    private readonly OrderService _orderService;

    public OrderServiceTests()
    {
        _inventory = Substitute.For<IInventory>();
        _paymentGateway = Substitute.For<IPaymentGateway>();
        _orderService = new OrderService(_inventory, _paymentGateway);
    }

    [Fact]
    public void ProcessOrder_ItemInStockAndPaymentSuccessful_ReturnsTrue()
    {
        // Arrange
        _inventory.IsInStock("apple").Returns(true);
        _paymentGateway.ProcessPayment(10.0).Returns(true);

        // Act
        var result = _orderService.ProcessOrder("apple", 10.0);

        // Assert
        Assert.True(result);
        _inventory.Received(1).ReduceStock("apple");
    }

    [Fact]
    public void ProcessOrder_ItemNotInStock_ReturnsFalse()
    {
        // Arrange
        _inventory.IsInStock("apple").Returns(false);

        // Act
        var result = _orderService.ProcessOrder("apple", 10.0);

        // Assert
        Assert.False(result);
        _inventory.DidNotReceive().ReduceStock("apple");
    }

    [Fact]
    public void ProcessOrder_PaymentFailed_ReturnsFalse()
    {
        // Arrange
        _inventory.IsInStock("apple").Returns(true);
        _paymentGateway.ProcessPayment(10.0).Returns(false);

        // Act
        var result = _orderService.ProcessOrder("apple", 10.0);

        // Assert
        Assert.False(result);
        _inventory.DidNotReceive().ReduceStock("apple");
    }
}

