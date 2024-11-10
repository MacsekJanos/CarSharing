using IVCFB2_HSZF_2024251.Model;

namespace IVCFB2_HSZF_2024251.Persistence.MsSql
{
    public interface ITripDataProvider
    {
        IEnumerable<Trip> GetAllTrips();
        Trip GetTripById(int id);
        void AddTrip(Trip trip);
        void UpdateTrip(Trip trip);
        void DeleteTrip(int id);

        void DeleteTrips(int[] ids);
        void DeleteAllTrip();
    }
}
