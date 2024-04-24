using Moq;

namespace XUnitTests.Moq;

public class OrderServiceMoqTests
{
    private readonly Mock<IInventory> _mockInventory;
    private readonly Mock<IPaymentGateway> _mockPaymentGateway;
    private readonly Mock<IMailService> _mailService;
    private readonly OrderService _orderService;

    public OrderServiceMoqTests()
    {
        _mockInventory = new Mock<IInventory>();
        _mockPaymentGateway = new Mock<IPaymentGateway>();
        _mailService = new Mock<IMailService>();
        _orderService = new OrderService(_mockInventory.Object, _mockPaymentGateway.Object, _mailService.Object);
    }

    [Fact]
    public void ProcessOrder_WhenItemInStockAndPaymentSuccess_ReturnsTrue()
    {
        _mockInventory.Setup(inv => inv.IsInStock(It.IsAny<string>())).Returns(true);
        _mockPaymentGateway.Setup(pg => pg.ProcessPayment(It.IsAny<double>())).Returns(true);

        bool result = _orderService.ProcessOrder("item1", 100.0);

        Assert.True(result);
        _mockInventory.Verify(inv => inv.ReduceStock("item1"), Times.Once);
        _mockPaymentGateway.Verify(p => p.ProcessPayment(100.0), Times.Once);
    }

    [Fact]
    public void ProcessOrder_WhenItemInStockAndPaymentFails_ReturnsFalse()
    {
        _mockInventory.Setup(inv => inv.IsInStock(It.IsAny<string>())).Returns(true);
        _mockPaymentGateway.Setup(pg => pg.ProcessPayment(It.IsAny<double>())).Returns(false);

        bool result = _orderService.ProcessOrder("item1", 100.0);

        Assert.False(result);
        _mockInventory.Verify(inv => inv.ReduceStock("item1"), Times.Never);
    }

    [Fact]
    public void ProcessOrder_WhenItemNotInStock_ReturnsFalse()
    {
        _mockInventory.Setup(inv => inv.IsInStock(It.IsAny<string>())).Returns(false);

        bool result = _orderService.ProcessOrder("item1", 100.0);

        Assert.False(result);
        _mockInventory.Verify(inv => inv.ReduceStock("item1"), Times.Never);
    }

    [Fact]
    public void ProcessOrder_WhenPaymentService_ThrowsException()
    {
        _mockInventory.Setup(inv => inv.IsInStock(It.IsAny<string>())).Returns(true);
        _mockPaymentGateway.Setup(pg => pg.ProcessPayment(It.IsAny<double>())).Throws(new Exception("Payment Failed"));
        
        bool result = _orderService.ProcessOrder("item1", 100.0);
        
        Assert.False(result);
        _mailService.Verify(m => m.SendErrorEmail(It.IsAny<Exception>()), Times.Once);
    }
}
