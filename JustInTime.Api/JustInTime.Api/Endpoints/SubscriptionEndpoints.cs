using JustInTime.Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace JustInTime.Api.Endpoints
{
    public static class SubscriptionEndpoints
    {
        private static readonly List<Subscription> subscriptions = new ()
        {
            new Subscription
            {
                Id = Guid.NewGuid(),
                Name = "Netflix",
                Cost = 15.99m,
                Cycle = Cycle.Monthly,
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                NextPaymentDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1)),
                Category = new Category { Id = Guid.NewGuid(), Name = "Entertainment" }
},
            new Subscription
            {
                Id = Guid.NewGuid(),
                Name = "Spotify",
                Cost = 9.99m,
                Cycle = Cycle.Monthly,
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                NextPaymentDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1)),
                Category = new Category { Id = Guid.NewGuid(), Name = "Music" }
            }
        };

        public static void RegisterSubscriptionEndpoitns(this IEndpointRouteBuilder app)
        {
            var subscriptions = app.MapGroup("/subscriptions");

            subscriptions.MapGet("/", GetAllSubscriptions);

            subscriptions.MapGet("/{id}", GetSubscriptionById)
                .WithName("GetSubscriptionById");

            subscriptions.MapPost("/", CreateSubscription);

            subscriptions.MapPut("/{id}", UpdateSubscription);

            subscriptions.MapDelete("/{id}", DeleteSubscription);
        }

        private static IResult GetAllSubscriptions()
        {
            return TypedResults.Ok(subscriptions);
        }

        private static Results<NotFound, Ok<Subscription>> GetSubscriptionById(Guid id)
        {
            var subscription = subscriptions.FirstOrDefault(s => s.Id == id);
            if(subscription == null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(subscription);
        }

        private static IResult CreateSubscription([FromBody] Subscription subscription)
        {
            subscription.Id = Guid.NewGuid();
            subscription.NextPaymentDate = subscription.Cycle switch
            {
                Cycle.Daily => subscription.StartDate.AddDays(1),
                Cycle.Monthly => subscription.StartDate.AddMonths(1),
                Cycle.Quarterly => subscription.StartDate.AddMonths(3),
                Cycle.Annually => subscription.StartDate.AddYears(1),
                _ => throw new ArgumentOutOfRangeException()
            };
            subscriptions.Add(subscription);
            return TypedResults.CreatedAtRoute(subscription, "GetSubscriptionById", new { id = subscription.Id });
        }

        private static Results<NotFound, NoContent> UpdateSubscription([FromBody] Subscription updatedSubscription, Guid id)
        {
            var subscription = subscriptions.FirstOrDefault(s => s.Id == id);
            if (subscription == null)
            {
                return TypedResults.NotFound();
            }

            subscription.Name = updatedSubscription.Name;
            subscription.Cost = updatedSubscription.Cost;
            subscription.Cycle = updatedSubscription.Cycle;
            subscription.StartDate = updatedSubscription.StartDate;
            subscription.NextPaymentDate = updatedSubscription.Cycle switch
            {
                Cycle.Daily => updatedSubscription.StartDate.AddDays(1),
                Cycle.Monthly => updatedSubscription.StartDate.AddMonths(1),
                Cycle.Quarterly => updatedSubscription.StartDate.AddMonths(3),
                Cycle.Annually => updatedSubscription.StartDate.AddYears(1),
                _ => throw new ArgumentOutOfRangeException()
            };
            subscription.CategoryId = updatedSubscription.CategoryId;
            subscription.Category = updatedSubscription.Category;

            return TypedResults.NoContent();
        }

        private static Results<NotFound, NoContent> DeleteSubscription(Guid id)
        {
            var subscription = subscriptions.FirstOrDefault(s => s.Id == id);
            if (subscription == null)
            {
                return TypedResults.NotFound();
            }
            subscriptions.Remove(subscription);
            return TypedResults.NoContent();
        }

    }
}
