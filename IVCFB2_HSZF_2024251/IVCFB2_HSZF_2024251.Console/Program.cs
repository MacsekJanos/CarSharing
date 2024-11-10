using IVCFB2_HSZF_2024251.Application;
using IVCFB2_HSZF_2024251.Model;
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

            Car car = carSharingServie.GetCarById(1);
        }
    }
}