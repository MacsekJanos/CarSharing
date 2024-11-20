using IVCFB2_HSZF_2024251.Model;
using IVCFB2_HSZF_2024251.Persistence.MsSql;

namespace IVCFB2_HSZF_2024251.Application
{
    public class CarSharingService : ICarSharingService
    {


        private readonly ICarSharingDataProvider dataProvider;

        public CarSharingService(ICarSharingDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;

        }

        public void DbWipe()
        {
            dataProvider.DbWipe();
        }

        public void DbSeed(string? path = null)
        {
            dataProvider.DbSeed();
        }

        public void Print<T>(IEnumerable<T> data)
        {
            dataProvider.Print(data);
        }

        public void ToList<T>(T data)
        {
            dataProvider.ToList<T>(data);
        }

        public string MostUsedCar()
        {
            return dataProvider.MostUsedCar();
        }

        public IEnumerable<string> Top10MostPayingCustomer()
        {
            return dataProvider.Top10MostPayingCustomer();
        }

        public double AvgDistance()
        {
            return dataProvider.AvgDistance();
        }

        public IEnumerable<Car> GetAllCars()
        {
            return dataProvider.GetAllCars();
        }

        public Car GetCarById(int id)
        {
            return dataProvider.GetCarById(id);
        }

        public void AddCar()
        {
            dataProvider.AddCar();
        }

        public void UpdateCar()
        {
            dataProvider.UpdateCar();
        }

        public void DeleteCar()
        {
            dataProvider.DeleteCar();
        }

        public void DeleteCars()
        {
            dataProvider.DeleteCars();
        }
        public void DeleteAllCar()
        {
            dataProvider.DeleteAllCar();
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return dataProvider.GetAllCustomers();
        }

        public Customer GetCustomerById(int id)
        {
            return dataProvider.GetCustomerById(id);
        }

        public void AddCustomer()
        {
            dataProvider.AddCustomer();
        }

        public void UpdateCustomer()
        {
            dataProvider.UpdateCustomer();
        }

        public void DeleteCustomer()
        {
            dataProvider.DeleteCustomer();
        }

        public void DeleteCustomers()
        {
            dataProvider.DeleteCustomers();
        }

        public void DeleteAllCustomer()
        {
            dataProvider.DeleteAllCustomer();
        }
        public IEnumerable<Trip> GetAllTrips()
        {
            return dataProvider.GetAllTrips();
        }

        public Trip GetTripById(int id)
        {
            return dataProvider.GetTripById(id);
        }

        public void AddTrip()
        {
            dataProvider.AddTrip();
        }

        public void UpdateTrip()
        {
            dataProvider.UpdateTrip();
        }

        public void DeleteTrip()
        {
            dataProvider.DeleteTrip();
        }

        public void DeleteTrips()
        {
            dataProvider.DeleteTrips();
        }

        public void DeleteAllTrip()
        {
            dataProvider.DeleteAllTrip();
        }
    }
}