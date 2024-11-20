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

    [Test]
    public void AddCar_InvalidTotalDistance_ShouldReturnFalse()
    {
        var car = new Car { TotalDistance = -1, Model = "Tesla" };
        var result = _service.AddCar(car);
        Assert.That(result, Is.False);
    }
}
