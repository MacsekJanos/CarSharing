namespace IVCFB2_HSZF_2024251.Persistence.MsSql
{
    public interface ICarSharingDataProvider : ICarDataProvider, ICustomerDataProvider, ITripDataProvider
    {
        void DbWipe();
        void DbSeed(string? path = null);

        void Print<T>(IEnumerable<T> data);

        void ToList<T>(T data);
        string MostUsedCar();
        IEnumerable<string> Top10MostPayingCustomer();
        double AvgDistance();
    }
}
