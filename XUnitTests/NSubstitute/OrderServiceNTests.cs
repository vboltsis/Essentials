using NSubstitute;
using TestProject;

namespace XUnitTests.NSubstitute;

public class OrderServiceNTests
{
    private readonly IInventory _inventory;
    private readonly IPaymentGateway _paymentGateway;
    private readonly IMailService _mailService;
    private readonly OrderService _orderService;

    public OrderServiceNTests()
    {
        _inventory = Substitute.For<IInventory>();
        _paymentGateway = Substitute.For<IPaymentGateway>();
        _mailService = Substitute.For<IMailService>();
        _orderService = new OrderService(_inventory, _paymentGateway, _mailService);
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
        _mailService.Received(1).SendMail("Purchased item apple for 10");
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
        _mailService.DidNotReceive().SendMail(Arg.Any<string>());
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
        _mailService.DidNotReceive().SendMail(Arg.Any<string>());
    }
}

