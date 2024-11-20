using IVCFB2_HSZF_2024251.Model;

namespace IVCFB2_HSZF_2024251.Application
{
    public interface ICarSharingService
    {

        //DB
        void DbWipe();
        void DbSeed(string? path = null);
        void Print<T>(IEnumerable<T> data);
        void ToList<T>(T data);
        string MostUsedCar();
        IEnumerable<string> Top10MostPayingCustomer();
        double AvgDistance();



        //Cars
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

        //Customers
        IEnumerable<Customer> GetAllCustomers();
        Customer GetCustomerById(int id);

        void CustomersToExcel();
        void AddCustomer();
        void UpdateCustomer();
        void DeleteCustomer();
        void DeleteCustomers();

        void DeleteAllCustomer();

        //Trips
        IEnumerable<Trip> GetAllTrips();
        Trip GetTripById(int id);

        void TripsToExcel();
        void AddTripFromConsole();
        void AddTrip(Trip trip);

        void UpdateTripFromConsole();
        void UpdateTrip(Trip trip);

        void DeleteTripFromConsole();
        void DeleteTrip(Trip trip);
        void DeleteTrips();
        void DeleteAllTrip();
    }
}
