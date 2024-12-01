using IVCFB2_HSZF_2024251.Model;

namespace IVCFB2_HSZF_2024251.Application
{
    public interface ICarSharingService
    {

        //DB
        void DbWipe();
        void DbSeed(string? path = null);
        void ToList<T>(T data);
        Car MostUsedCar();
        IEnumerable<Customer> Top10MostPayingCustomer();
        double AvgDistance();



        //Cars
        IEnumerable<Car> GetAllCars();
        Car GetCarById(int id);

        void CarsToExcel();
        bool AddCar(Car car);
        void UpdateCarFromConsole();
        bool UpdateCar(Car car);
        bool DeleteCar(Car car);

        void DeleteCars();
        void DeleteAllCar();

        //Customers
        IEnumerable<Customer> GetAllCustomers();
        Customer GetCustomerById(int id);

        void CustomersToExcel();
        void AddCustomerFromConsole();
        bool AddCustomer(Customer customer);

        void UpdateCustomerFromConsole();
        bool UpdateCustomer(Customer customer);

        void DeleteCustomerFromConsole();
        bool DeleteCustomer(Customer customer);
        void DeleteCustomers();

        void DeleteAllCustomer();

        //Trips
        IEnumerable<Trip> GetAllTrips();
        Trip GetTripById(int id);

        void TripsToExcel();
        void AddTripFromConsole();
        bool AddTrip(Trip trip);

        void UpdateTripFromConsole();
        bool UpdateTrip(Trip trip);

        void DeleteTripFromConsole();
        bool DeleteTrip(Trip trip);
        void DeleteTrips();
        void DeleteAllTrip();
    }
}
