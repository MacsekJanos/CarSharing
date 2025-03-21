﻿using IVCFB2_HSZF_2024251.Model;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Reflection;
using System.Xml.Linq;

namespace IVCFB2_HSZF_2024251.Persistence.MsSql
{
    public class CarSharingDataProvider : ICarSharingDataProvider
    {
        private CarSharingDbContext context;

        private readonly DBEvent dbEvent = new DBEvent();
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

            dbEvent.OnActionCompleted("Az adatbázis sikeresen törlődött");
        }
        public void DbSeed(string? path = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = Path.Combine(Directory.GetCurrentDirectory(), "carsharing.xml");
            }

            if (!File.Exists(path))
            {
                Console.WriteLine("File nem található a megadott útnovalon: " + path);
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
            dbEvent.OnActionCompleted("Az adatbázis sikeresen feltőltődött");
        }
        public void ToList<T>(T data)
        {
            if (data == null) return;

            if (data is IEnumerable<object> enumerable)
            {
                foreach (var item in enumerable)
                {
                    ToList(item);
                }
            }
            else
            {
                Type type = data.GetType();
                PropertyInfo[] properties = type.GetProperties();

                foreach (var property in properties)
                {
                    var value = property.GetValue(data);
                    if (property.PropertyType.IsPrimitive || property.PropertyType == typeof(string) || property.Name.EndsWith("Id"))
                    {
                        Console.WriteLine($"{property.Name}: {value}");
                    }
                }
                Console.WriteLine(new string('-', 20));
            }
        }
        public Car MostUsedCar()
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

            if (mostUsedCar == null)
            {
                return null;
            }

            Car result = context.Cars.FirstOrDefault(c => c.Id == mostUsedCar.CarId);

            if (result != null)
            {
                Console.WriteLine($"A legtöbbet futott autó: {result.Model}, Idáig megtett út: {mostUsedCar.TotalDistance:F2}Km");
            }
            return result;
        }

        public IEnumerable<Customer> Top10MostPayingCustomer()
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

            var customerNamesWithPayments = context.Customers
                .Where(c => mostPayingCustomers.Select(m => m.CustomerId).Contains(c.Id))
                .Select(c => new
                {
                    c.Name,
                    TotalPaid = mostPayingCustomers.First(m => m.CustomerId == c.Id).TotalPaid
                })
                    .ToList();

            Console.WriteLine("\nA 10 legtöbbet fizető vásárló: \n");
            int i = 1;
            foreach (var customer in customerNamesWithPayments)
            {
                Console.WriteLine($"{i++}. {customer.Name}, Összesen fizetett: {customer.TotalPaid:F2} euro");
            }
            return context.Customers
                .Where(c => mostPayingCustomers.Select(m => m.CustomerId).Contains(c.Id))
                .ToList();

        }

        public double AvgDistance()
        {
            var result = context.Cars.Average(c => c.TotalDistance);
            Console.WriteLine("Az autók átlagos futása: " + result + "Km");
            return result;
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

        public void CarsToExcel()
        {
            var cars = context.Cars.ToList();
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var projectDirectory = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\.."));
            var csvFilePath = Path.Combine(projectDirectory, "Exports", "cars.csv");

            Directory.CreateDirectory(Path.GetDirectoryName(csvFilePath));

            using (var writer = new StreamWriter(csvFilePath))
            {
                var properties = typeof(Car).GetProperties();
                writer.WriteLine(string.Join(";", properties.Select(p => p.Name)));

                foreach (var car in cars)
                {
                    var values = properties.Select(p => p.GetValue(car)?.ToString()?.Replace(";", ",") ?? string.Empty);
                    writer.WriteLine(string.Join(";", values));
                }
            }

            dbEvent.OnActionCompleted("Az autók adatai sikeresen exportálva lettek a cars.csv fájlba.");
        }

        public void AddCar(Car car)
        {
            if (car == null || string.IsNullOrEmpty(car.Model) || car.TotalDistance < 0)
            {
                return;
            }
            car.DistanceSinceLastMaintenance = 0;
            context.Cars.Add(car);
            context.SaveChanges();
        }

        public void UpdateCar(Car car)
        {
            if (car == null || string.IsNullOrEmpty(car.Model) || car.TotalDistance < 0 || car.DistanceSinceLastMaintenance < 0)
            {
                return;
            }
            context.Cars.Update(car);
            context.SaveChanges();
            return;
        }


        public void DeleteCar(Car car)
        {
            if (car == null)
            {
                return;
            }
            context.Cars.Remove(car);
            context.SaveChanges();

        }

        //public void DeleteCars()
        //{
        //    Console.WriteLine("Válassza ki a törölni kívánt autókat ID alapján, vesszővel elválasztva:");
        //    var cars = context.Cars.ToList();
        //    ToList(cars);
        //    string input = Console.ReadLine();
        //    var carIds = input.Split(',')
        //                      .Select(id => int.TryParse(id.Trim(), out int carId) ? carId : (int?)null)
        //                      .Where(id => id.HasValue)
        //                      .Select(id => id.Value)
        //                      .ToList();

        //    if (!carIds.Any() || !carIds.All(id => cars.Any(c => c.Id == id)))
        //    {
        //        dbEvent.OnActionCompleted("Érvénytelen autó ID-k.");
        //        return;
        //    }

        //    var carsToDelete = context.Cars.Where(c => carIds.Contains(c.Id)).ToList();
        //    context.Cars.RemoveRange(carsToDelete);
        //    context.SaveChanges();
        //    dbEvent.OnActionCompleted("Az autók sikeresen törölve lettek!");
        //}

        public void DeleteAllCar()
        {
            context.Cars.RemoveRange(context.Cars);
            context.SaveChanges();
            dbEvent.OnActionCompleted("Az összes autó törölve lett");
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
        public void CustomersToExcel()
        {
            var customers = context.Customers.ToList();
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var projectDirectory = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\..\"));
            var csvFilePath = Path.Combine(projectDirectory, "Exports", "customers.csv");

            Directory.CreateDirectory(Path.GetDirectoryName(csvFilePath));

            using (var writer = new StreamWriter(csvFilePath))
            {
                var properties = typeof(Customer).GetProperties();
                writer.WriteLine(string.Join(";", properties.Select(p => p.Name)));

                foreach (var customer in customers)
                {
                    var values = properties.Select(p => p.GetValue(customer)?.ToString()?.Replace(";", ",") ?? string.Empty);
                    writer.WriteLine(string.Join(";", values));
                }
            }

            dbEvent.OnActionCompleted("A vásárlók adatai sikeresen exportálva lettek a customers.csv fájlba.");
        }

        public void AddCustomer(Customer customer)
        {
            if (customer == null || string.IsNullOrEmpty(customer.Name) || customer.Balance < 0)
            {
                return;
            }

            context.Customers.Add(customer);
            context.SaveChanges();
        }

        public void UpdateCustomer(Customer customer)
        {
            if (customer == null || string.IsNullOrEmpty(customer.Name) || customer.Balance < 0)
            {
                return;
            }

            context.Customers.Update(customer);
            context.SaveChanges();
        }

        public void DeleteCustomer(Customer customer)
        {
            if (customer == null)
            {
                return;
            }

            context.Customers.Remove(customer);
            context.SaveChanges();
        }

        //public void DeleteCustomers()
        //{
        //    Console.WriteLine("Válassza ki a törölni kívánt vásárlókat ID alapján, vesszővel elválasztva:");
        //    var customers = context.Customers.ToList();
        //    ToList(customers);

        //    string input = Console.ReadLine();
        //    var customerIds = input.Split(',')
        //                           .Select(id => int.TryParse(id.Trim(), out int customerId) ? customerId : (int?)null)
        //                           .Where(id => id.HasValue)
        //                           .Select(id => id.Value)
        //                           .ToList();

        //    if (!customerIds.Any() || !customerIds.All(id => customers.Any(c => c.Id == id)))
        //    {
        //        dbEvent.OnActionCompleted("Érvénytelen vásárló ID-k.");
        //        return;
        //    }

        //    var customersToDelete = context.Customers.Where(c => customerIds.Contains(c.Id)).ToList();
        //    context.Customers.RemoveRange(customersToDelete);
        //    context.SaveChanges();
        //    dbEvent.OnActionCompleted("A vásárlók sikeresen törölve lettek!");
        //}
        public void DeleteAllCustomer()
        {
            context.Customers.RemoveRange(context.Customers);
            context.SaveChanges();
            dbEvent.OnActionCompleted("Az összes várásló törölve lett!");
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

        public void TripsToExcel()
        {
            var trips = context.Trips.ToList();
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var projectDirectory = Path.GetFullPath(Path.Combine(baseDirectory, @"..\..\.."));
            var csvFilePath = Path.Combine(projectDirectory, "Exports", "trips.csv");

            Directory.CreateDirectory(Path.GetDirectoryName(csvFilePath));

            using (var writer = new StreamWriter(csvFilePath))
            {
                var properties = typeof(Trip).GetProperties()
                                             .Where(p => !typeof(Customer).IsAssignableFrom(p.PropertyType) && !typeof(Car).IsAssignableFrom(p.PropertyType))
                                             .ToArray();
                writer.WriteLine(string.Join(";", properties.Select(p => p.Name)));

                foreach (var trip in trips)
                {
                    var values = properties.Select(p => p.GetValue(trip)?.ToString()?.Replace(";", ",") ?? string.Empty);
                    writer.WriteLine(string.Join(";", values));
                }
            }

            dbEvent.OnActionCompleted("Az utak adatai sikeresen exportálva lettek a trips.csv fájlba.");
        }

        public void AddTrip(Trip trip)
        {
            if (trip == null || trip.Distance <= 0 || trip.Cost <= 0)
            {
                return;
            }

            var customer = context.Customers.Find(trip.CustomerId);
            var car = context.Cars.Find(trip.CarId);

            if (customer == null || car == null || customer.Balance < trip.Cost)
            {
                return;
            }

            context.Trips.Add(trip);
            customer.Balance -= trip.Cost;
            car.TotalDistance += trip.Distance;
            car.DistanceSinceLastMaintenance += trip.Distance;

            context.SaveChanges();
        }


        public void UpdateTrip(Trip trip)
        {
            if (trip == null || trip.Distance <= 0 || trip.Cost <= 0)
            {
                return;
            }

            var customer = context.Customers.Find(trip.CustomerId);
            var car = context.Cars.Find(trip.CarId);

            if (customer == null || car == null || customer.Balance < trip.Cost)
            {
                return;
            }

            customer.Balance -= trip.Cost;
            car.TotalDistance += trip.Distance;
            car.DistanceSinceLastMaintenance += trip.Distance;

            context.Trips.Update(trip);
            context.SaveChanges();
        }

        public void DeleteTrip(Trip trip)
        {
            if (trip == null)
            {
                return;
            }

            context.Trips.Remove(trip);
            context.SaveChanges();
        }

        //public void DeleteTrips()
        //{
        //    Console.WriteLine("Válassza ki a törölni kívánt utakat ID alapján, vesszővel elválasztva:");
        //    var trips = context.Trips.ToList();
        //    ToList(trips);

        //    string input = Console.ReadLine();
        //    var tripIds = input.Split(',')
        //                       .Select(id => int.TryParse(id.Trim(), out int tripId) ? tripId : (int?)null)
        //                       .Where(id => id.HasValue)
        //                       .Select(id => id.Value)
        //                       .ToList();

        //    if (!tripIds.Any() || !tripIds.All(id => trips.Any(t => t.Id == id)))
        //    {
        //        dbEvent.OnActionCompleted("Érvénytelen út ID-k.");
        //        return;
        //    }

        //    var tripsToDelete = context.Trips.Where(t => tripIds.Contains(t.Id)).ToList();
        //    context.Trips.RemoveRange(tripsToDelete);
        //    context.SaveChanges();
        //    dbEvent.OnActionCompleted("Az utak sikeresen törölve lettek!");
        //}
        public void DeleteAllTrip()
        {
            context.Trips.RemoveRange(context.Trips);
            context.SaveChanges();
            dbEvent.OnActionCompleted("Az összes út törölve lett");
        }
    }
}