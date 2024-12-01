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

        public void ToList<T>(T data)
        {
            dataProvider.ToList<T>(data);
        }

        public Car MostUsedCar()
        {
            return dataProvider.MostUsedCar();
        }

        public IEnumerable<Customer> Top10MostPayingCustomer()
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
        public bool AddCar(Car car)
        {
            if (car == null || string.IsNullOrEmpty(car.Model) || car.TotalDistance < 0)
            {
                return false;
            }
            dataProvider.AddCar(car);
            return true;
        }


        public bool UpdateCar(Car car)
        {
            if (car == null || string.IsNullOrEmpty(car.Model) || car.TotalDistance < 0 || car.DistanceSinceLastMaintenance < 0)
            {
                return false;
            }
            dataProvider.UpdateCar(car);
            return true;
        }

        public bool DeleteCar(Car car)
        {
            if (car == null)
            {
                return false;
            }
            dataProvider.DeleteCar(car);
            return true;
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

        public bool AddCustomer(Customer customer)
        {
            if (customer == null || string.IsNullOrEmpty(customer.Name) || customer.Balance < 0)
            {
                return false;
            }

            dataProvider.AddCustomer(customer);
            return true;
        }

        public bool UpdateCustomer(Customer customer)
        {
            if (customer == null || string.IsNullOrEmpty(customer.Name) || customer.Balance < 0)
            {
                return false;
            }
            dataProvider.UpdateCustomer(customer);
            return true;
        }
        public bool DeleteCustomer(Customer customer)
        {
            if (customer == null)
            {
                return false;
            }
            dataProvider.DeleteCustomer(customer);
            return true;
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

        public bool AddTrip(Trip trip)
        {
            if (trip == null || trip.Distance <= 0 || trip.Cost <= 0)
            {
                return false;
            }

            var customer = dataProvider.GetCustomerById(trip.CustomerId);
            var car = dataProvider.GetCarById(trip.CarId);

            if (customer == null || car == null || customer.Balance < trip.Cost)
            {
                return false;
            }

            dataProvider.AddTrip(trip);
            return true;
        }

        public void UpdateTripFromConsole()
        {
            dataProvider.UpdateTripFromConsole();
        }
        public bool UpdateTrip(Trip trip)
        {
            if (trip == null || trip.Distance <= 0 || trip.Cost <= 0)
            {
                return false;
            }

            var customer = dataProvider.GetCustomerById(trip.CustomerId);
            var car = dataProvider.GetCarById(trip.CarId);

            if (customer == null || car == null || customer.Balance < trip.Cost)
            {
                return false;
            }

            dataProvider.UpdateTripFromConsole();
            return true;
        }

        public void DeleteTripFromConsole()
        {
            dataProvider.DeleteTripFromConsole();
        }
        public bool DeleteTrip(Trip trip)
        {
            if (trip == null)
            {
                return false;
            }
            dataProvider.DeleteTrip(trip);
            return true;
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