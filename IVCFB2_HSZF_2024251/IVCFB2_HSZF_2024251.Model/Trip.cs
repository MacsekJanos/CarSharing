

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IVCFB2_HSZF_2024251.Model
{
    public class Trip
    {
        public Trip() { }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CarId { get; set; }
        public int CustomerId { get; set; }
        public double Distance { get; set; }
        public double Cost { get; set; }
        public virtual Car Car { get; set; }
        public virtual Customer Customer { get; set; }

    }

}
