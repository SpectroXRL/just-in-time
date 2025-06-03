using JustInTime.Api.Models;

namespace JustInTime.Api.Contracts.Response
{
    public record SubscriptionResponse
    (
        Guid Id,
        string Name,
        decimal Cost,
        Cycle Cycle,
        DateOnly StartDate,
        DateOnly? NextPaymentDate,
        string CategoryName
    );
}
