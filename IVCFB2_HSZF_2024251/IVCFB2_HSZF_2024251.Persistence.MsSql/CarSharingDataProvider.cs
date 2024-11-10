using IVCFB2_HSZF_2024251.Model;

namespace IVCFB2_HSZF_2024251.Persistence.MsSql
{
    public class CarSharingDataProvider : ICarSharingDataProvider
    {
        private CarSharingDbContext context;
        public CarSharingDataProvider(CarSharingDbContext context)
        {
            this.context = context;
        }

        public void DbWipe()
        {
            context.Trips.RemoveRange(context.Trips);
            context.Cars.RemoveRange(context.Cars);
            context.Customers.RemoveRange(context.Customers);
            context.SaveChanges();
        }
        public void DbSeed(string? path = null)
        {

        }
        public IEnumerable<Car> GetAllCars()
        {
            return context.Cars.ToList();
        }

        public Car GetCarById(int id)
        {
            return context.Cars.Find(id);
        }

        public void AddCar(Car car)
        {
            context.Cars.Add(car);
            context.SaveChanges();
        }

        public void UpdateCar(Car car)
        {
            context.Cars.Update(car);
            context.SaveChanges();
        }

        public void DeleteCar(int id)
        {
            var car = context.Cars.Find(id);
            if (car != null)
            {
                context.Cars.Remove(car);
                context.SaveChanges();
            }
        }

        public void DeleteCars(int[] ids)
        {
            var cars = context.Cars.Where(c => ids.Contains(c.Id)).ToList();
            if (cars != null)
            {
                context.Cars.RemoveRange(cars);
                context.SaveChanges();
            }
        }

        public void DeleteAllCar()
        {
            context.Cars.RemoveRange(context.Cars);
            context.SaveChanges();
        }
        public IEnumerable<Customer> GetAllCustomers()
        {
            return context.Customers.ToList();
        }

        public Customer GetCustomerById(int id)
        {
            return context.Customers.Find(id);
        }

        public void AddCustomer(Customer customer)
        {
            context.Customers.Add(customer);
            context.SaveChanges();
        }

        public void UpdateCustomer(Customer customer)
        {
            context.Customers.Update(customer);
            context.SaveChanges();
        }

        public void DeleteCustomer(int id)
        {
            var customer = context.Customers.Find(id);
            if (customer != null)
            {
                context.Customers.Remove(customer);
                context.SaveChanges();
            }
        }

        public void DeleteCustomers(int[] ids)
        {
            var customers = context.Customers.Where(c => ids.Contains(c.Id)).ToList();
            if (customers != null)
            {
                context.Customers.RemoveRange(customers);
                context.SaveChanges();
            }
        }
        public void DeleteAllCustomer()
        {
            context.Customers.RemoveRange(context.Customers);
            context.SaveChanges();
        }
        public IEnumerable<Trip> GetAllTrips()
        {
            return context.Trips.ToList();
        }

        public Trip GetTripById(int id)
        {
            return context.Trips.Find(id);
        }

        public void AddTrip(Trip trip)
        {
            context.Trips.Add(trip);
            context.SaveChanges();
        }

        public void UpdateTrip(Trip trip)
        {
            context.Trips.Update(trip);
            context.SaveChanges();
        }

        public void DeleteTrip(int id)
        {
            var trip = context.Trips.Find(id);
            if (trip != null)
            {
                context.Trips.Remove(trip);
                context.SaveChanges();
            }
        }

        public void DeleteTrips(int[] ids)
        {
            var trips = context.Trips.Where(c => ids.Contains(c.Id)).ToList();
            if (trips != null)
            {
                context.Trips.RemoveRange(trips);
                context.SaveChanges();
            }
        }
        public void DeleteAllTrip()
        {
            context.Trips.RemoveRange(context.Trips);
            context.SaveChanges();
        }
    }
}
