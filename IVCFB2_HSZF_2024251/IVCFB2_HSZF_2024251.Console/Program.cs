using IVCFB2_HSZF_2024251.Application;
using IVCFB2_HSZF_2024251.Persistence.MsSql;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IVCFB2_HSZF_2024251
{
    public class Program
    {

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

            DisplayMenu(
            new string[]
            {
            "Bolvasás",
            "Lekérdezések",
            "Adatbázis műveletek"
            },
            [
              () => DisplayMenu(new string[]{
                "Adatok beolvasása xml-ből"
                },
                [
                () => carSharingServie.DbSeed()
                ]
                ),
              () => DisplayMenu(new string[]
                {
                    "Autók listázása",
                    "Vásárlók listázása",
                    "Utazások listázása",
                },
                [

                ]
                ),
              () => DisplayMenu(
                  new string[]{
                      "Autók",
                      "Vásárlók",
                      "Utazások",
                      "Adatbázis törlése"
                  },
                  [
                      () => DisplayMenu(new string[]
                         {
                          "Új autó felvétele",
                          "Autó módosítása",
                          "Autó törlése",
                          "Autók törlése",
                          "Összes autó törlése",
                         },
                         [

                         ]
                          ),

                      () => DisplayMenu(new string[]
                         {
                          "Új vásárló felvétele",
                          "Vásárló módosítása",
                          "Vásárló törlése",
                          "Vásárlók törlése",
                          "Összes vásárló törlése",
                         },
                         [

                         ]
                          ),

                      () => DisplayMenu(new string[]
                         {
                          "Új utazás",
                          "Utazás módosítása",
                          "Utazás törlése",
                          "Utazások törlése",
                          "Összes utazás törlése",
                         },
                         [

                         ]
                          ),
                      () => carSharingServie.DbWipe()
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
    }
}