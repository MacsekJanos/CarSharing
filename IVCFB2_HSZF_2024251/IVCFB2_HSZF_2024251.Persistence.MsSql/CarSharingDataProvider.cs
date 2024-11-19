using IVCFB2_HSZF_2024251.Model;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Xml.Linq;

namespace IVCFB2_HSZF_2024251.Persistence.MsSql
{
    public class CarSharingDataProvider : ICarSharingDataProvider
    {
        private CarSharingDbContext context;
        public CarSharingDataProvider(CarSharingDbContext context)
        {
            this.context = context;
        }

        //DB Actions
        public void DbWipe()
        {
            context.Trips.RemoveRange(context.Trips);
            context.Cars.RemoveRange(context.Cars);
            context.Customers.RemoveRange(context.Customers);
            context.SaveChanges();

            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Trips', RESEED, 0);");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Cars', RESEED, 0);");
            context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Customers', RESEED, 0);");
        }
        public void DbSeed(string? path = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "carsharing.xml");
            }

            if (!File.Exists(path))
            {
                Console.WriteLine("File not found: " + path);
                return;
            }

            var xdoc = XDocument.Load(path);

            foreach (var item in xdoc.Descendants("Car"))
            {
                context.Cars.Add(new Car
                {
                    Model = item.Element("Model").Value,
                    TotalDistance = Double.Parse(item.Element("TotalDistance").Value, CultureInfo.InvariantCulture),
                    DistanceSinceLastMaintenance = Double.Parse(item.Element("DistanceSinceLastMaintenance").Value, CultureInfo.InvariantCulture),

                });
                context.SaveChanges();
            }

            foreach (var item in xdoc.Descendants("Customer"))
            {
                context.Customers.Add(new Customer
                {
                    Name = item.Element("Name").Value,
                    Balance = Double.Parse(item.Element("Balance").Value, CultureInfo.InvariantCulture),
                });
                context.SaveChanges();
            }
            foreach (var item in xdoc.Descendants("Trip"))
            {
                context.Trips.Add(new Trip
                {
                    CarId = int.Parse(item.Element("CarId").Value),
                    CustomerId = int.Parse(item.Element("CustomerId").Value),
                    Distance = Double.Parse(item.Element("Distance").Value, CultureInfo.InvariantCulture),
                    Cost = Double.Parse(item.Element("Cost").Value, CultureInfo.InvariantCulture),
                });
                context.SaveChanges();

            }
        }
        public string MostUsedCar()
        {
            var mostUsedCar = context.Trips
            .GroupBy(t => t.CarId)
             .Select(g => new
             {
                 CarId = g.Key,
                 TotalDistance = g.Sum(t => t.Distance)
             })
              .OrderByDescending(g => g.TotalDistance)
               .FirstOrDefault();

            return context.Cars.Where(c => c.Id == mostUsedCar.CarId).Select(c => c.Model).FirstOrDefault();
        }

        public IEnumerable<string> Top10MostPayingCustomer()
        {
            var mostPayingCustomers = context.Trips
                .GroupBy(t => t.CustomerId)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    TotalPaid = g.Sum(t => t.Cost),
                })
                .OrderByDescending(g => g.TotalPaid)
                .Take(10);

            return context.Customers
                .Where(c => mostPayingCustomers.Select(m => m.CustomerId).Contains(c.Id))
                .Select(c => c.Name)
                .ToList();


        }

        public double AvgDistance()
        {
            return context.Cars.Average(c => c.TotalDistance);
        }

        //Car Actions
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

        //CustomerActions
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

        //Trip Actions
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
