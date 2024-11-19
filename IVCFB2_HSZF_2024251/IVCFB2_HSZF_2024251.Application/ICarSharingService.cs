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
        void AddCar();
        void UpdateCar(Car car);
        void DeleteCar(int id);

        void DeleteCars(int[] ids);
        void DeleteAllCar();

        //Customers
        IEnumerable<Customer> GetAllCustomers();
        Customer GetCustomerById(int id);
        void AddCustomer();
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(int id);
        void DeleteCustomers(int[] ids);

        void DeleteAllCustomer();

        //Trips
        IEnumerable<Trip> GetAllTrips();
        Trip GetTripById(int id);
        void AddTrip();
        void UpdateTrip(Trip trip);
        void DeleteTrip(int id);
        void DeleteTrips(int[] ids);
        void DeleteAllTrip();
    }
}
