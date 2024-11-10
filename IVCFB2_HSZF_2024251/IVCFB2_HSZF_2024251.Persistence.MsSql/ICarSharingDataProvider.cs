namespace IVCFB2_HSZF_2024251.Persistence.MsSql
{
    public interface ICarSharingDataProvider : ICarDataProvider, ICustomerDataProvider, ITripDataProvider
    {
        void DbWipe();
        void DbSeed(string? path = null);
    }
}
