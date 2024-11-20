using IVCFB2_HSZF_2024251.Model;

namespace IVCFB2_HSZF_2024251.Persistence.MsSql
{
    public interface ICarDataProvider
    {
        IEnumerable<Car> GetAllCars();
        Car GetCarById(int id);
        void AddCar();
        void UpdateCar();
        void DeleteCar();

        void DeleteCars();
        void DeleteAllCar();
    }
}
