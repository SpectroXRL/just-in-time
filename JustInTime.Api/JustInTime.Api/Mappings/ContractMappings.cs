using JustInTime.Api.Contracts.Requests;
using JustInTime.Api.Contracts.Response;
using JustInTime.Api.Endpoints;
using JustInTime.Api.Models;

namespace JustInTime.Api.Mappings
{
    public static class ContractMappings
    {
        public static Subscription MapToSubscription(this CreateSubscriptionRequest request)
        {
            return new Subscription
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Cost = request.Cost,
                Cycle = request.Cycle,
                StartDate = request.StartDate,
                NextPaymentDate = request.Cycle switch
                {
                    Cycle.Daily => request.StartDate.AddDays(1),
                    Cycle.Monthly => request.StartDate.AddMonths(1),
                    Cycle.Quarterly => request.StartDate.AddMonths(3),
                    Cycle.Annually => request.StartDate.AddYears(1),
                    _ => throw new ArgumentOutOfRangeException()
                },
                CategoryId = SubscriptionEndpoints.categories.FirstOrDefault(category => category.Name == request.Category).Id,
            };
        }

        public static Subscription MapToSubscription(this UpdateSubscriptionRequest request, Guid id)
        {
            return new Subscription
            {
                Id = id,
                Name = request.Name,
                Cost = request.Cost,
                Cycle = request.Cycle,
                StartDate = request.StartDate,
                NextPaymentDate = request.Cycle switch
                {
                    Cycle.Daily => request.StartDate.AddDays(1),
                    Cycle.Monthly => request.StartDate.AddMonths(1),
                    Cycle.Quarterly => request.StartDate.AddMonths(3),
                    Cycle.Annually => request.StartDate.AddYears(1),
                    _ => throw new ArgumentOutOfRangeException()
                },
                CategoryId = SubscriptionEndpoints.categories.FirstOrDefault(category => category.Name == request.Category).Id,
            };
        }

        public static SubscriptionResponse MapToSubscriptionResponse(this Subscription subscription)
        {
            return new SubscriptionResponse(
                subscription.Id,
                subscription.Name,
                subscription.Cost,
                subscription.Cycle,
                subscription.StartDate,
                subscription.NextPaymentDate,
                SubscriptionEndpoints.categories.FirstOrDefault(category => category.Id == subscription.CategoryId)?.Name ?? "Unknown"
            );
        }

        public static SubscriptionResponses MapToSubscriptionResponses(this IEnumerable<Subscription> subscriptions)
        {
            decimal monthlyCost = 0;

            foreach (var subscription in subscriptions){
                monthlyCost += subscription.Cycle switch
                {
                    Cycle.Daily => subscription.Cost * 30,
                    Cycle.Monthly => subscription.Cost,
                    Cycle.Quarterly => subscription.Cost / 3,
                    Cycle.Annually => subscription.Cost / 12,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }

            return new SubscriptionResponses(
                subscriptions.Select(subscription => subscription.MapToSubscriptionResponse()).ToList(),
                monthlyCost,
                subscriptions.Count()
                );
        }
    }
}
