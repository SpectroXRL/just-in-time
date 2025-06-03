using JustInTime.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace JustInTime.Api.Contracts.Requests
{
    public record CreateSubscriptionRequest
    (
        [Required, StringLength(100, MinimumLength = 1)] string Name,
        [Required, Range(0.01, double.MaxValue)] decimal Cost,
        [Required] Cycle Cycle,
        [Required] DateOnly StartDate,
        [Required] string Category
    );
}
