namespace JustInTime.Api.Contracts.Response
{
    public record SubscriptionResponses
    (
        List<SubscriptionResponse> data,
        decimal monthlyCost,
        int totalSubscriptions
    );
}
