using IVCFB2_HSZF_2024251.Application;
using IVCFB2_HSZF_2024251.Model;
using IVCFB2_HSZF_2024251.Persistence.MsSql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IVCFB2_HSZF_2024251
{
    public class Program
    {
        private static bool isDatabaseSeeded = false;

        private static DBEvent dbEvent = new DBEvent();
        private static void Main(string[] args)
        {

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddScoped<CarSharingDbContext>();
                    services.AddSingleton<ICarSharingDataProvider, CarSharingDataProvider>();
                    services.AddSingleton<ICarSharingService, CarSharingService>();
                })
                .Build();

            host.Start();
            using IServiceScope serviceScope = host.Services.CreateScope();
            var carSharingServie = host.Services.GetService<ICarSharingService>();

            //Cars

            void AddCarFromConsole()
            {
                Console.WriteLine("Adja meg a modelt:");
                string model = Console.ReadLine();
                if (string.IsNullOrEmpty(model))
                {
                    Console.WriteLine("Érvénytelen bemenet. A modell nem lehet üres.");
                    return;
                }
                Console.WriteLine("Adja meg az eddig megtett távot:");
                string distanceInput = Console.ReadLine();
                if (string.IsNullOrEmpty(distanceInput) || !double.TryParse(distanceInput, out double totalDistance) || totalDistance < 0)
                {
                    Console.WriteLine("Érvénytelen bemenet. A távnak nullánál nem kisebb számnak kell lennie.");
                    return;
                }
                var car = new Car
                {
                    Model = model,
                    TotalDistance = totalDistance,
                    DistanceSinceLastMaintenance = 0
                };
                dbEvent.OnActionCompleted("Az új autó sikeresen bekerült a flottába!");
                carSharingServie.AddCar(car);
            }
            void UpdateCarFromConsole()
            {
                Console.WriteLine("Válasszon autót ID alapján:");
                var cars = carSharingServie.GetAllCars().ToList();
                carSharingServie.ToList(cars);
                if (!int.TryParse(Console.ReadLine(), out int carId) || !cars.Any(c => c.Id == carId))
                {
                    dbEvent.OnActionCompleted("Érvénytelen autó ID.");
                    return;
                }
                var car = carSharingServie.GetCarById(carId);
                if (car == null)
                {
                    dbEvent.OnActionCompleted("Nem található autó a megadott ID-val.");
                    return;
                }
                Console.WriteLine("Adja meg az új modellt:");
                string newModel = Console.ReadLine();
                Console.WriteLine("Adja meg az eddig megtett távot:");
                if (!double.TryParse(Console.ReadLine(), out double totalDistance) || totalDistance < 0)
                {
                    dbEvent.OnActionCompleted("Érvénytelen bemenet, a távnak nullánál nem kisebb számnak kell lennie.");
                    return;
                }
                Console.WriteLine("Adja meg a szerviz óta megtett távot:");
                if (!double.TryParse(Console.ReadLine(), out double distanceSinceLastMaintenance) || distanceSinceLastMaintenance <= 0)
                {
                    dbEvent.OnActionCompleted("Érvénytelen bemenet, a távnak nullánál nem kisebb számnak kell lennie.");
                    return;
                }
                car.Model = newModel;
                car.TotalDistance = totalDistance;
                car.DistanceSinceLastMaintenance = distanceSinceLastMaintenance > 200 ? 0 : distanceSinceLastMaintenance;
                carSharingServie.UpdateCar(car);
                dbEvent.OnActionCompleted("Az autó sikeresen frissítve lett!");
            }
            void DeleteCarFromConsole()
            {
                Console.WriteLine("Válassza ki a törölni kívánt autót ID alapján:");
                var cars = carSharingServie.GetAllCars();
                carSharingServie.ToList(cars);
                if (!int.TryParse(Console.ReadLine(), out int carId) || !cars.Any(c => c.Id == carId))
                {
                    dbEvent.OnActionCompleted("Érvénytelen autó ID.");
                    return;
                }
                var car = carSharingServie.GetCarById(carId);
                if (car == null)
                {
                    dbEvent.OnActionCompleted("Nem található autó a megadott ID-val.");
                    return;
                }
                carSharingServie.DeleteCar(car);
                dbEvent.OnActionCompleted("Az autó sikeresen törölve lett!");
            }

            //Customers
            void AddCustomerFromConsole()
            {
                Console.WriteLine("Adja meg a vásárló nevét:");
                string name = Console.ReadLine();

                if (string.IsNullOrEmpty(name))
                {
                    dbEvent.OnActionCompleted("Érvénytelen bemenet, a név nem lehet üres.");
                    return;
                }

                Console.WriteLine("Adja meg a vásárló egyenlegét:");
                if (!double.TryParse(Console.ReadLine(), out double balance) || balance < 0)
                {
                    dbEvent.OnActionCompleted("Érvénytelen bemenet, az egyenlegnek nullánál nagyobb számnak kell lennie.");
                    return;
                }

                var customer = new Customer
                {
                    Name = name,
                    Balance = balance
                };

                carSharingServie.AddCustomer(customer);
                dbEvent.OnActionCompleted("Az új vásárló sikeresen fel lett véve!");
            }
            void UpdateCustomerFromConsole()
            {
                Console.WriteLine("Válasszon vásárlót ID alapján:");
                var customers = carSharingServie.GetAllCustomers().ToList();
                carSharingServie.ToList(customers);

                if (!int.TryParse(Console.ReadLine(), out int customerId) || !customers.Any(c => c.Id == customerId))
                {
                    dbEvent.OnActionCompleted("Érvénytelen vásárló ID.");
                    return;
                }

                var customer = carSharingServie.GetCustomerById(customerId);
                if (customer == null)
                {
                    dbEvent.OnActionCompleted("Nem található vásárló a megadott ID-val.");
                    return;
                }

                Console.WriteLine("Adja meg az új nevet:");
                string newName = Console.ReadLine();

                if (string.IsNullOrEmpty(newName))
                {
                    dbEvent.OnActionCompleted("Érvénytelen bemenet, a név nem lehet üres.");
                    return;
                }

                Console.WriteLine("Adja meg az új egyenleget:");
                if (!double.TryParse(Console.ReadLine(), out double balance) || balance < 0)
                {
                    dbEvent.OnActionCompleted("Érvénytelen bemenet, az egyenlegnek nullánál nagyobb számnak kell lennie.");
                    return;
                }

                customer.Name = newName;
                customer.Balance = balance;

                carSharingServie.UpdateCustomer(customer);
                dbEvent.OnActionCompleted("A vásárló adatai sikeresen frissültek!");
            }
            void DeleteCustomerFromConsole()
            {
                Console.WriteLine("Válassza ki a törölni kívánt vásárlót ID alapján:");
                var customers = carSharingServie.GetAllCustomers().ToList();
                carSharingServie.ToList(customers);

                if (!int.TryParse(Console.ReadLine(), out int customerId) || !customers.Any(c => c.Id == customerId))
                {
                    dbEvent.OnActionCompleted("Érvénytelen vásárló ID.");
                    return;
                }

                var customer = carSharingServie.GetCustomerById(customerId);
                if (customer == null)
                {
                    dbEvent.OnActionCompleted("Nem található vásárló a megadott ID-val.");
                    return;
                }

                carSharingServie.DeleteCustomer(customer);
                dbEvent.OnActionCompleted("A vásárló sikeresen törölve lett!");
            }


            DisplayMenu(
            new string[]
            {
            "Bolvasás",
            "Listás nézetek",
            "Adatbázis műveletek"
            },
            [
              () => DisplayMenu(new string[]{
                "Adatok beolvasása xml-ből"
                },
                [
                () => SeedDatabase(carSharingServie)
                ]
                ),
              () => DisplayMenu(new string[]
                {
                    "Autók listázása",
                    "Vásárlók listázása",
                    "Utazások listázása",
                },
                [
                    () => carSharingServie.ToList(carSharingServie.GetAllCars()),
                    () => carSharingServie.ToList(carSharingServie.GetAllCustomers()),
                    () => carSharingServie.ToList(carSharingServie.GetAllTrips())
                ]
                ),
              () => DisplayMenu(
                  new string[]{
                      "Autók",
                      "Vásárlók",
                      "Utazások",
                      "Lekérdezések",
                      "Adatbázis törlése"
                  },
                  [
                      () => DisplayMenu(new string[]
                         {
                          "Új autó felvétele",
                          "Autó módosítása",
                          "Autó törlése",
                          "Autók törlése",
                          "Autók exportálása",
                          "Összes autó törlése",
                         },
                         [
                             () => AddCarFromConsole(),
                             () => UpdateCarFromConsole(),
                             () => DeleteCarFromConsole(),
                             () => carSharingServie.DeleteCars(),
                             () => carSharingServie.CarsToExcel(),
                             () => carSharingServie.DeleteAllCar(),
                         ]
                          ),

                      () => DisplayMenu(new string[]
                         {
                          "Új vásárló felvétele",
                          "Vásárló módosítása",
                          "Vásárló törlése",
                          "Vásárlók törlése",
                          "Vásárlók exportálása",
                          "Összes vásárló törlése",
                         },
                         [
                             () => AddCustomerFromConsole(),
                             () => UpdateCustomerFromConsole(),
                             () => DeleteCustomerFromConsole(),
                             () => carSharingServie.DeleteCustomers(),
                             () => carSharingServie.CustomersToExcel(),
                             () => carSharingServie.DeleteAllCustomer(),

                         ]
                          ),

                      () => DisplayMenu(new string[]
                         {
                          "Új utazás",
                          "Utazás módosítása",
                          "Utazás törlése",
                          "Utazások törlése",
                          "Utazások exportálása",
                          "Összes utazás törlése",
                         },
                         [
                             () => carSharingServie.AddTripFromConsole(),
                             () => carSharingServie.UpdateTripFromConsole(),
                             () => carSharingServie.DeleteTripFromConsole(),
                             () => carSharingServie.DeleteTrips(),
                             () => carSharingServie.TripsToExcel(),
                             () => carSharingServie.DeleteAllTrip(),
                         ]
                          ),
                           () => DisplayMenu(new string[]
                      {
                          "Legtöbbet futott jármű",
                          "Top 10 legtöbbet fizető ügyfél",
                          "Autók átlagos futása",
                      },
                      [
                          () => carSharingServie.MostUsedCar(),
                          () => carSharingServie.Top10MostPayingCustomer(),
                          () => carSharingServie.AvgDistance(),
                      ]),
                      () => WipeDatabase(carSharingServie)
                  ]
               )
            ]
            );
        }
        static void DisplayMenu(string[] menus, Action[] actions)
        {
            bool exit = false;
            int y = 5;
            while (!exit)
            {
                Console.Clear();
                Console.SetCursorPosition(50, 3);
                Console.WriteLine("Válassz egy menüpontot:");
                for (int i = 0; i < menus.Length; i++)
                {
                    Console.SetCursorPosition(50, y);
                    Console.WriteLine($"{i + 1}. {menus[i]}");
                    y += 2;
                }
                Console.SetCursorPosition(50, y);
                Console.WriteLine($"{menus.Length + 1}. Kilépés");
                Console.SetCursorPosition(50, y += 2);
                Console.Write("Add meg a választott menüpont számát: ");
                string input = Console.ReadLine();

                int selectedIndex;
                if (int.TryParse(input, out selectedIndex) && selectedIndex >= 1 && selectedIndex <= menus.Length + 1)
                {
                    if (selectedIndex == menus.Length + 1)
                    {
                        Console.WriteLine("Kilépés...");
                        exit = true;
                    }
                    else
                    {
                        actions[selectedIndex - 1]();
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("Érvénytelen választás, próbáld újra.");
                    Console.ReadKey();
                }
                y = 5;
            }
        }
        static void SeedDatabase(ICarSharingService carSharingService)
        {
            if (isDatabaseSeeded)
            {
                Console.WriteLine("Az adatbázis már fel lett töltve.");
            }
            else
            {
                carSharingService.DbSeed();
                isDatabaseSeeded = true;
            }
        }

        static void WipeDatabase(ICarSharingService carSharingService)
        {
            carSharingService.DbWipe();
            isDatabaseSeeded = false;
        }
    }
}