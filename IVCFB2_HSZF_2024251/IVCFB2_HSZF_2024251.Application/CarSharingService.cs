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

        //Db
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

        //Cars

        public IEnumerable<Car> GetAllCars()
        {
            return dataProvider.GetAllCars();
        }

        public Car GetCarById(int id)
        {
            return dataProvider.GetCarById(id);
        }

        public void CarsToExcel()
        {
            dataProvider.CarsToExcel();
        }
        public void AddCarFromConsole()
        {
            dataProvider.AddCarFromConsole();
        }
        public void AddCar(Car car)
        {
            dataProvider.AddCar(car);
        }

        public void UpdateCarFromConsole()
        {
            dataProvider.UpdateCarFromConsole();
        }

        public void UpdateCar(Car car)
        {
            dataProvider.UpdateCar(car);
        }
        public void DeleteCarFromConsole()
        {
            dataProvider.DeleteCarFromConsole();
        }
        public void DeleteCar(Car car)
        {
            dataProvider.DeleteCar(car);
        }

        public void DeleteCars()
        {
            dataProvider.DeleteCars();
        }
        public void DeleteAllCar()
        {
            dataProvider.DeleteAllCar();
        }

        //Customers

        public IEnumerable<Customer> GetAllCustomers()
        {
            return dataProvider.GetAllCustomers();
        }

        public Customer GetCustomerById(int id)
        {
            return dataProvider.GetCustomerById(id);


        }

        public void CustomersToExcel()
        {
            dataProvider.CustomersToExcel();
        }

        public void AddCustomerFromConsole()
        {
            dataProvider.AddCustomerFromConsole();
        }
        public void AddCustomer(Customer customer)
        {
            dataProvider.AddCustomer(customer);
        }

        public void UpdateCustomerFromConsole()
        {
            dataProvider.UpdateCustomerFromConsole();
        }
        public void UpdateCustomer(Customer customer)
        {
            dataProvider.UpdateCustomer(customer);
        }

        public void DeleteCustomerFromConsole()
        {
            dataProvider.DeleteCustomerFromConsole();
        }
        public void DeleteCustomer(Customer customer)
        {
            dataProvider.DeleteCustomer(customer);
        }

        public void DeleteCustomers()
        {
            dataProvider.DeleteCustomers();
        }

        public void DeleteAllCustomer()
        {
            dataProvider.DeleteAllCustomer();
        }

        //Trips
        public IEnumerable<Trip> GetAllTrips()
        {
            return dataProvider.GetAllTrips();
        }

        public Trip GetTripById(int id)
        {
            return dataProvider.GetTripById(id);
        }

        public void TripsToExcel()
        {
            dataProvider.TripsToExcel();
        }

        public void AddTripFromConsole()
        {
            dataProvider.AddTripFromConsole();
        }
        public void AddTrip(Trip trip)
        {
            dataProvider.AddTrip(trip);
        }

        public void UpdateTripFromConsole()
        {
            dataProvider.UpdateTripFromConsole();
        }
        public void UpdateTrip(Trip trip)
        {
            dataProvider.UpdateTrip(trip);
        }

        public void DeleteTripFromConsole()
        {
            dataProvider.DeleteTripFromConsole();
        }
        public void DeleteTrip(Trip trip)
        {
            dataProvider.DeleteTrip(trip);
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