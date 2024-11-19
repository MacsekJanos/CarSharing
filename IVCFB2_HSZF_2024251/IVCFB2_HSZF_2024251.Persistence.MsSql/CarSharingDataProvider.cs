using IVCFB2_HSZF_2024251.Model;
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
            dbEvent.OnActionCompleted("Az adatbázis sikeresen feltőltődött");
        }
        public void Print<T>(IEnumerable<T> data)
        {
            int i = 1;
            foreach (var item in data)
            {
                Console.WriteLine($"{i++}. {item?.ToString()}");
            }
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

            Car result = context.Cars.Where(c => c.Id == mostUsedCar.CarId).FirstOrDefault();
            Console.WriteLine("A legtöbbet futott kocsi: " + result.Model + "Idáig megtett út: " + result.TotalDistance + "Km");
            return "";
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

            Console.WriteLine("\nA 10 legtöbbet fizető vásárló: \n");
            return context.Customers
                .Where(c => mostPayingCustomers.Select(m => m.CustomerId).Contains(c.Id))
                .Select(c => c.Name)
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

        public void AddCar()
        {

            Console.WriteLine("Adja meg a modelt:");

            string model = Console.ReadLine();

            Console.WriteLine("Adja meg az eddig megtett távot:");
            if (!double.TryParse(Console.ReadLine(), out double totalDistance) || totalDistance < 0)
            {
                dbEvent.OnActionCompleted("Érvénytelen bemenet a, távnak nullánál nagyobb számnak kell lennie.");
                return;
            }

            var car = new Car
            {
                Model = model,
                TotalDistance = totalDistance,
                DistanceSinceLastMaintenance = 0
            };
            dbEvent.OnActionCompleted("Az új autó sikeresen bekerült a flottába!");
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

        public void AddCustomer()
        {
            Console.WriteLine("Adja meg a vásárló nevét:");

            string name = Console.ReadLine();

            Console.WriteLine("Adja meg a vásárló áegyenlegét:");
            if (!double.TryParse(Console.ReadLine(), out double balance) || balance < 0)
            {
                dbEvent.OnActionCompleted("Érvénytelen bemenet, az egyenlegnek nullánál nagyobb számnak kell lennie.");
                return;
            }

            var customer = new Customer
            {
                Name = name,
                Balance = balance,

            };
            dbEvent.OnActionCompleted("Az új vásárló sikeresen fel lett véve!");
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

        public void AddTrip()
        {

            Console.WriteLine("Válasszon Vásárlót ID alapján:");
            var customers = context.Customers.ToList();
            ToList(customers);
            if (!int.TryParse(Console.ReadLine(), out int customerId) || !customers.Any(c => c.Id == customerId))
            {
                dbEvent.OnActionCompleted("Érvénytelen ID.");
                return;
            }

            var selectedCustomer = context.Customers.Find(customerId);
            if (selectedCustomer.Balance < 40)
            {
                dbEvent.OnActionCompleted("Nincs elég fedezet, a minimum 40 euro");
                return;
            }

            Console.WriteLine("Válasszon autót ID alapján:");
            var cars = context.Cars.ToList();
            ToList(cars);
            if (!int.TryParse(Console.ReadLine(), out int carId) || !cars.Any(c => c.Id == carId))
            {
                dbEvent.OnActionCompleted("Érvénytelen autó ID.");
                return;
            }
            var selectedCar = context.Cars.Find(carId);

            Console.WriteLine("Adj meg a tervezett távolságot (km):");
            if (!double.TryParse(Console.ReadLine(), out double distance) || distance <= 0)
            {
                dbEvent.OnActionCompleted("Érvénytelen táv, az útnak nullánál nagyobb számnak kell lennie.");
                return;
            }

            double tripCost = 0.5 + (distance * 0.35);
            if (selectedCustomer.Balance < tripCost)
            {
                dbEvent.OnActionCompleted("Nincs elég fedezet az útra");
                return;
            }

            var trip = new Trip
            {
                CarId = carId,
                CustomerId = customerId,
                Distance = distance,
                Cost = tripCost
            };

            context.Trips.Add(trip);

            selectedCustomer.Balance -= tripCost;
            selectedCar.TotalDistance += distance;
            selectedCar.DistanceSinceLastMaintenance += distance;

            context.SaveChanges();

            dbEvent.OnActionCompleted($"Sikeres út felvétel: Vásárló: {selectedCustomer.Name}, Autó: {selectedCar.Model}, Ár: {trip.Cost}€, Távolság: {trip.Distance} km.");


            if (selectedCar.DistanceSinceLastMaintenance >= 200 || new Random().Next(1, 101) <= 20)
            {
                dbEvent.OnActionCompleted($"Az autó {selectedCar.Model} szervízt igényelt!");
                selectedCar.DistanceSinceLastMaintenance = 0;
            }

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
