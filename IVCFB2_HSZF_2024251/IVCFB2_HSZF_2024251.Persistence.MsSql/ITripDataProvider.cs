using IVCFB2_HSZF_2024251.Model;

namespace IVCFB2_HSZF_2024251.Persistence.MsSql
{
    public interface ITripDataProvider
    {
        IEnumerable<Trip> GetAllTrips();
        Trip GetTripById(int id);

        void TripsToExcel();
        void AddTrip(Trip trip);
        void UpdateTrip(Trip trip);
        void DeleteTrip(Trip trip);

        //void DeleteTrips();
        void DeleteAllTrip();
    }
}
