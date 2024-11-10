namespace IVCFB2_HSZF_2024251.Model
{
    public class Trip
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public int CustomerId { get; set; }
        public double Distance { get; set; }
        public double Cost { get; set; }

        public Car Car { get; set; }
        public Customer Customer { get; set; }
    }

}
