using IVCFB2_HSZF_2024251.Model;

namespace IVCFB2_HSZF_2024251.Persistence.MsSql
{
    public interface ICarDataProvider
    {
        IEnumerable<Car> GetAllCars();
        Car GetCarById(int id);

        void CarsToExcel();
        void AddCarFromConsole();
        void AddCar(Car car);

        void UpdateCarFromConsole();
        void UpdateCar(Car car);
        void DeleteCarFromConsole();
        void DeleteCar(Car car);

        void DeleteCars();
        void DeleteAllCar();
    }
}
