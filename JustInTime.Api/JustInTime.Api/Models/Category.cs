namespace JustInTime.Api.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    }
}
