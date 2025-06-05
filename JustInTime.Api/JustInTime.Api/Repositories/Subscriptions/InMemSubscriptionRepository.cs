using JustInTime.Api.Contracts.Requests;
using JustInTime.Api.Mappings;
using JustInTime.Api.Models;
using JustInTime.Api.Repositories.Categories;

namespace JustInTime.Api.Repositories.Subscriptions
{
    public class InMemSubscriptionRepository : ISubscriptionRepository
    {
        private readonly List<Subscription> subscriptions;

        public InMemSubscriptionRepository(ICategoryRepository categoryRepository)
        {
            List<Category> categories = categoryRepository.GetCategoriesAsync().Result;

            subscriptions = new()
            {
                new Subscription
                {
                    Id = Guid.NewGuid(),
                    Name = "Netflix",
                    Cost = 15.99m,
                    Cycle = Cycle.Monthly,
                    StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    NextPaymentDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1)),
                    CategoryId = categories.First(category => category.Name == "Entertainment").Id
                },
                new Subscription
                {
                    Id = Guid.NewGuid(),
                    Name = "Spotify",
                    Cost = 9.99m,
                    Cycle = Cycle.Monthly,
                    StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                    NextPaymentDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1)),
                    CategoryId = categories.First(category => category.Name == "Music").Id
                }
            };
        }

        public async Task<List<Subscription>> GetSubscriptionsAsync()
        {
            return await Task.FromResult(subscriptions);
        }

        public async Task<Subscription> GetSubscriptionAsync(Guid id)
        {
            return await Task.FromResult(subscriptions.First(subscription => subscription.Id == id));
        }

        public async Task CreateSubscriptionAsync(Subscription subscription)
        {
            subscriptions.Add(subscription);
            await Task.CompletedTask;
        }

        public async Task UpdateSubscriptionAsync(Guid id, Subscription updatedSubscription)
        {
          int index = subscriptions.FindIndex(subscription => subscription.Id == id);
          subscriptions[index] = updatedSubscription;
          await Task.CompletedTask;
        }

        public async Task DeleteSubscriptionAsync(Guid id)
        {
            int index = subscriptions.FindIndex(subscription => subscription.Id == id);
            subscriptions.RemoveAt(index);
            await Task.CompletedTask;
        }
    }
}
