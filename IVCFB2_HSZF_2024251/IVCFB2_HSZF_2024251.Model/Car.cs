using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IVCFB2_HSZF_2024251.Model
{
    public class Car
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Model { get; set; }
        public double TotalDistance { get; set; }
        public double DistanceSinceLastMaintenance { get; set; }

        public virtual ICollection<Trip> Trips { get; set; }

        public Car()
        {
            Trips = new HashSet<Trip>();
        }
    }

}