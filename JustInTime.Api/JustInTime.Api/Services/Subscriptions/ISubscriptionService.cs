using JustInTime.Api.Models;

namespace JustInTime.Api.Services.Subscriptions
{
    public interface ISubscriptionService
    {
        Task<(List<Subscription> subscriptions, bool isValidRequest)> GetSubscriptionsAsync(Guid? categoryID, string? orderOption, string? sortOrder);
        Task<(Subscription subscription, bool isValidRequest)> GetSubscriptionAsync(Guid id);
        Task<(Subscription subscription, bool isValidRequest)> CreateSubscriptionAsync(Subscription subscription);
        Task<bool> UpdateSubscriptionAsync(Guid id, Subscription updatedSubscription);
        Task<bool> DeleteSubscriptionAsync(Guid id);
    }
}