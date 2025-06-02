using System.ComponentModel.DataAnnotations;

namespace JustInTime.Api.Models
{
    public class Subscription
    {
        [Required]
        public Guid Id { get; set; }

        [Required, StringLength(100, MinimumLength = 1)]
        public required string Name { get; set; }

        [Required, Range(0.01, double.MaxValue, ErrorMessage = "Cost must be a positive value.")]
        public decimal Cost { get; set; }

        [Required]
        public Cycle Cycle { get; set; }

        [Required]
        public DateOnly StartDate { get; set; }

        [Required]
        public DateOnly? NextPaymentDate { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        public Category Category { get; set; } = null!;
    }
}
