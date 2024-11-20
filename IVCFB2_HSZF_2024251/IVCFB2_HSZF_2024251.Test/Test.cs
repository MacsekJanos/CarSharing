using IVCFB2_HSZF_2024251.Application;
using IVCFB2_HSZF_2024251.Model;
using IVCFB2_HSZF_2024251.Persistence.MsSql;
using Moq;
using NUnit.Framework;

namespace IVCFB2_HSZF_2024251.Test
{
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
        public void AddCar_InvalidTotalDistance_ShouldNotAddCar()
        {
            // Arrange
            var car = new Car { TotalDistance = -1 }; // Explicitly invalid value

            // Act
            _service.AddCar(car);

            // Assert
            _mockDataProvider.Verify(m => m.AddCar(It.Is<Car>(c => c.Model == car.Model && c.TotalDistance <= -1)), Times.Once);
        }

        [Test]
        public void AddCar_ValidCar_ShouldAddCarToDatabase()
        {
            // Arrange
            var car = new Car
            {
                Model = "Tesla Model X",
                TotalDistance = 3000.0
            };

            // Act
            _service.AddCar(car);

            // Assert
            _mockDataProvider.Verify(m => m.AddCar(It.Is<Car>(c => c.Model == car.Model && c.TotalDistance == car.TotalDistance)), Times.Once);
        }

    }
}