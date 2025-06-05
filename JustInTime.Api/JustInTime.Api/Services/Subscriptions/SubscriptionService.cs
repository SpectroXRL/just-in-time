using JustInTime.Api.Endpoints;
using JustInTime.Api.Models;
using JustInTime.Api.Repositories.Categories;
using JustInTime.Api.Repositories.Subscriptions;

namespace JustInTime.Api.Services.Subscriptions
{
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ICategoryRepository _categoryRepository;
        public SubscriptionService(ISubscriptionRepository subscriptionRepository, ICategoryRepository categoryRepository)
        {
            _subscriptionRepository = subscriptionRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<(List<Subscription> subscriptions, bool isValidRequest)> GetSubscriptionsAsync(Guid? categoryID,
            string? orderOption, string? sortOrder)
        {
            var subscriptions = await _subscriptionRepository.GetSubscriptionsAsync();

            subscriptions = categoryID == null
                ? subscriptions
                : subscriptions.Where(s => s.CategoryId == categoryID).ToList();

            //Subscription Ordering
            SubscriptionOrderOptions orderOpt = SubscriptionOrderOptions.Name;
            if (orderOption != null)
            {
                if (!Enum.TryParse<SubscriptionOrderOptions>(orderOption, true, out orderOpt)
                    ||
                    sortOrder != null && !string.Equals(sortOrder, "asc", StringComparison.OrdinalIgnoreCase)
                    && !string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase))
                {
                    return (new List<Subscription>(), false);
                }
            };

            var isDescending = string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase);
            subscriptions = orderOpt switch
            {
                SubscriptionOrderOptions.Name => isDescending ? subscriptions.OrderByDescending(subscription => subscription.Name).ToList()
                : subscriptions.OrderBy(subscription => subscription.Name).ToList(),

                SubscriptionOrderOptions.StartDate => isDescending ? subscriptions.OrderByDescending(subscription => subscription.StartDate).ToList()
                : subscriptions.OrderBy(subscription => subscription.StartDate).ToList(),

                SubscriptionOrderOptions.Cost => isDescending ? subscriptions.OrderByDescending(subscription => subscription.Cost).ToList()
                : subscriptions.OrderBy(subscription => subscription.Cost).ToList(),

                _ => throw new NotImplementedException(),
            };

            return (subscriptions, true);
        }

        public async Task<(Subscription subscription, bool isValidRequest)> GetSubscriptionAsync(Guid id)
        {
            var subscription = await _subscriptionRepository.GetSubscriptionAsync(id);
            if (subscription == null)
            {
                return (new Subscription{}, false);
            }
            return (subscription, true);
        }

        public async Task<(Subscription subscription, bool isValidRequest)> CreateSubscriptionAsync(Subscription subscription) 
        {
            await _subscriptionRepository.CreateSubscriptionAsync(subscription);
            return (subscription, true);
        }

        public async Task<bool> UpdateSubscriptionAsync(Guid id, Subscription updatedSubscription)
        {
            var subscription = (await _subscriptionRepository.GetSubscriptionsAsync()).FirstOrDefault(s => s.Id == id);
            if (subscription == null)
            {
                return await Task.FromResult(false);
            }
            await _subscriptionRepository.UpdateSubscriptionAsync(id, updatedSubscription);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteSubscriptionAsync(Guid id)
        {
            var subscription = (await _subscriptionRepository.GetSubscriptionsAsync()).FirstOrDefault(s => s.Id == id);
            if (subscription == null)
            {
                return await Task.FromResult(false);
            }
            await _subscriptionRepository.DeleteSubscriptionAsync(id);
            return await Task.FromResult(true);
        }
    }
}
