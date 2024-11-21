using IVCFB2_HSZF_2024251.Model;

namespace IVCFB2_HSZF_2024251.Persistence.MsSql
{
    public interface ICarSharingDataProvider : ICarDataProvider, ICustomerDataProvider, ITripDataProvider
    {
        void DbWipe();
        void DbSeed(string? path = null);
        void ToList<T>(T data);
        Car MostUsedCar();
        IEnumerable<Customer> Top10MostPayingCustomer();
        double AvgDistance();
    }
}
