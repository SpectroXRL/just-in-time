namespace JustInTime.Api.Models
{
    public class Subscription
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public decimal Cost { get; set; }
        public Cycle Cycle { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly NextPaymentDate { get; set; }
        public required Category Category { get; set; }
    }
}
