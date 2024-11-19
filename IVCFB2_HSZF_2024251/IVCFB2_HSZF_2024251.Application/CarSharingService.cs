using IVCFB2_HSZF_2024251.Model;
using IVCFB2_HSZF_2024251.Persistence.MsSql;

namespace IVCFB2_HSZF_2024251.Application
{
    public class CarSharingService : ICarSharingService
    {
        private readonly DBEvent dbEvent;

        private readonly ICarSharingDataProvider dataProvider;

        public CarSharingService(ICarSharingDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
            this.dbEvent = new DBEvent();
        }

        public void DbWipe()
        {
            dataProvider.DbWipe();
            dbEvent.OnActionCompleted("Az adatbázis sikeresen törlődött");
        }

        public void DbSeed(string? path = null)
        {
            dataProvider.DbSeed();
            dbEvent.OnActionCompleted("Az adatbázis sikeresen feltöltődött");
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

        public void AddCar(Car car)
        {
            dataProvider.AddCar(car);
        }

        public void UpdateCar(Car car)
        {
            dataProvider.UpdateCar(car);
        }

        public void DeleteCar(int id)
        {
            dataProvider.DeleteCar(id);
        }

        public void DeleteCars(int[] ids)
        {
            dataProvider.DeleteCars(ids);
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

        public void AddCustomer(Customer customer)
        {
            dataProvider.AddCustomer(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            dataProvider.UpdateCustomer(customer);
        }

        public void DeleteCustomer(int id)
        {
            dataProvider.DeleteCustomer(id);
        }

        public void DeleteCustomers(int[] ids)
        {
            dataProvider.DeleteCustomers(ids);
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

        public void AddTrip(Trip trip)
        {
            dataProvider.AddTrip(trip);
        }

        public void UpdateTrip(Trip trip)
        {
            dataProvider.UpdateTrip(trip);
        }

        public void DeleteTrip(int id)
        {
            dataProvider.DeleteTrip(id);
        }

        public void DeleteTrips(int[] ids)
        {
            dataProvider.DeleteTrips(ids);
        }

        public void DeleteAllTrip()
        {
            dataProvider.DeleteAllTrip();
        }
    }
}