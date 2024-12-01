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
        bool UpdateCar(Car car);
        bool DeleteCar(Car car);

        void DeleteCars();
        void DeleteAllCar();

        //Customers
        IEnumerable<Customer> GetAllCustomers();
        Customer GetCustomerById(int id);

        void CustomersToExcel();
        bool AddCustomer(Customer customer);
        bool UpdateCustomer(Customer customer);
        bool DeleteCustomer(Customer customer);
        void DeleteCustomers();

        void DeleteAllCustomer();

        //Trips
        IEnumerable<Trip> GetAllTrips();
        Trip GetTripById(int id);

        void TripsToExcel();
        bool AddTrip(Trip trip);

        bool UpdateTrip(Trip trip);

        bool DeleteTrip(Trip trip);
        void DeleteTrips();
        void DeleteAllTrip();
    }
}
