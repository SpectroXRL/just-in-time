using JustInTime.Api.Models;

namespace JustInTime.Api.Repositories.Subscriptions
{
    public interface ISubscriptionRepository
    {
        Task<Subscription> GetSubscriptionAsync(Guid id);
        Task<List<Subscription>> GetSubscriptionsAsync();
        Task CreateSubscriptionAsync(Subscription subscription);
        Task UpdateSubscriptionAsync(Guid id, Subscription updatedSubscription);
        Task DeleteSubscriptionAsync(Guid id);
    }
}