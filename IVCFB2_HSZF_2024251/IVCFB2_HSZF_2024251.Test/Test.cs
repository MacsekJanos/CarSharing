using IVCFB2_HSZF_2024251.Application;
using IVCFB2_HSZF_2024251.Model;
using IVCFB2_HSZF_2024251.Persistence.MsSql;
using Moq;
using NUnit.Framework;

[TestFixture]
public class CarSharingServiceTests
{
    private Mock<ICarSharingDataProvider> _mockDataProvider;
    private CarSharingService _service;

    [SetUp]
    public void Setup()
    {
        _mockDataProvider = new Mock<ICarSharingDataProvider>();
        _service = new CarSharingService(_mockDataProvider.Object);
    }

    // Car tests
    [Test]
    public void AddCar_InvalidTotalDistance_ShouldReturnFalse()
    {
        var car = new Car { TotalDistance = -1, Model = "Tesla" };
        var result = _service.AddCar(car);
        Assert.That(result, Is.False);
    }

    [Test]
    public void AddCar_ValidCar_ShouldReturnTrue()
    {
        var car = new Car { TotalDistance = 100, Model = "Tesla" };
        var result = _service.AddCar(car);
        Assert.That(result, Is.True);
    }

    [Test]
    public void UpdateCar_InvalidTotalDistance_ShouldReturnFalse()
    {
        var car = new Car { TotalDistance = -1, Model = "Tesla", DistanceSinceLastMaintenance = 50 };
        var result = _service.UpdateCar(car);
        Assert.That(result, Is.False);
    }

    [Test]
    public void UpdateCar_ValidCar_ShouldReturnTrue()
    {
        var car = new Car { TotalDistance = 100, Model = "Tesla", DistanceSinceLastMaintenance = 50 };
        var result = _service.UpdateCar(car);
        Assert.That(result, Is.True);
    }

    [Test]
    public void DeleteCar_NullCar_ShouldReturnFalse()
    {
        Car car = null;
        var result = _service.DeleteCar(car);
        Assert.That(result, Is.False);
    }

    [Test]
    public void DeleteCar_ValidCar_ShouldReturnTrue()
    {
        var car = new Car { TotalDistance = 100, Model = "Tesla" };
        var result = _service.DeleteCar(car);
        Assert.That(result, Is.True);
    }

    // Customer tests
    [Test]
    public void AddCustomer_InvalidBalance_ShouldReturnFalse()
    {
        var customer = new Customer { Balance = -1, Name = "John Doe" };
        var result = _service.AddCustomer(customer);
        Assert.That(result, Is.False);
    }

    [Test]
    public void AddCustomer_ValidCustomer_ShouldReturnTrue()
    {
        var customer = new Customer { Balance = 100, Name = "John Doe" };
        var result = _service.AddCustomer(customer);
        Assert.That(result, Is.True);
    }

    [Test]
    public void UpdateCustomer_InvalidBalance_ShouldReturnFalse()
    {
        var customer = new Customer { Balance = -1, Name = "John Doe" };
        var result = _service.UpdateCustomer(customer);
        Assert.That(result, Is.False);
    }

    [Test]
    public void UpdateCustomer_ValidCustomer_ShouldReturnTrue()
    {
        var customer = new Customer { Balance = 100, Name = "John Doe" };
        var result = _service.UpdateCustomer(customer);
        Assert.That(result, Is.True);
    }

    [Test]
    public void DeleteCustomer_NullCustomer_ShouldReturnFalse()
    {
        Customer customer = null;
        var result = _service.DeleteCustomer(customer);
        Assert.That(result, Is.False);
    }

    [Test]
    public void DeleteCustomer_ValidCustomer_ShouldReturnTrue()
    {
        var customer = new Customer { Balance = 100, Name = "John Doe" };
        var result = _service.DeleteCustomer(customer);
        Assert.That(result, Is.True);
    }

    // Trip tests
    [Test]
    public void AddTrip_InvalidDistance_ShouldReturnFalse()
    {
        var trip = new Trip { Distance = -1, Cost = 100, CustomerId = 1, CarId = 1 };
        var result = _service.AddTrip(trip);
        Assert.That(result, Is.False);
    }

    [Test]
    public void AddTrip_ValidTrip_ShouldReturnTrue()
    {
        var trip = new Trip { Distance = 100, Cost = 50, CustomerId = 1, CarId = 1 };
        var customer = new Customer { Id = 1, Balance = 100 };
        var car = new Car { Id = 1, Model = "Tesla" };

        _mockDataProvider.Setup(dp => dp.GetCustomerById(1)).Returns(customer);
        _mockDataProvider.Setup(dp => dp.GetCarById(1)).Returns(car);

        var result = _service.AddTrip(trip);
        Assert.That(result, Is.True);
    }

    [Test]
    public void UpdateTrip_InvalidDistance_ShouldReturnFalse()
    {
        var trip = new Trip { Distance = -1, Cost = 100, CustomerId = 1, CarId = 1 };
        var result = _service.UpdateTrip(trip);
        Assert.That(result, Is.False);
    }

    [Test]
    public void UpdateTrip_ValidTrip_ShouldReturnTrue()
    {
        var trip = new Trip { Distance = 100, Cost = 50, CustomerId = 1, CarId = 1 };
        var customer = new Customer { Id = 1, Balance = 100 };
        var car = new Car { Id = 1, Model = "Tesla" };

        _mockDataProvider.Setup(dp => dp.GetCustomerById(1)).Returns(customer);
        _mockDataProvider.Setup(dp => dp.GetCarById(1)).Returns(car);

        var result = _service.UpdateTrip(trip);
        Assert.That(result, Is.True);
    }

    [Test]
    public void DeleteTrip_NullTrip_ShouldReturnFalse()
    {
        Trip trip = null;
        var result = _service.DeleteTrip(trip);
        Assert.That(result, Is.False);
    }

    [Test]
    public void DeleteTrip_ValidTrip_ShouldReturnTrue()
    {
        var trip = new Trip { Distance = 100, Cost = 50, CustomerId = 1, CarId = 1 };
        var result = _service.DeleteTrip(trip);
        Assert.That(result, Is.True);
    }
}