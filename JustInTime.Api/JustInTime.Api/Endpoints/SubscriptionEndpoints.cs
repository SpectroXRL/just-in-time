using JustInTime.Api.Contracts.Requests;
using JustInTime.Api.Contracts.Response;
using JustInTime.Api.Mappings;
using JustInTime.Api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace JustInTime.Api.Endpoints
{
    public static class SubscriptionEndpoints
    {
        public static readonly List<Category> categories = new()
        {
            new Category
            {
                Id = Guid.NewGuid(),
                Name = "Entertainment"
            },
            new Category
            {
                Id = Guid.NewGuid(),
                Name = "Music"
            },
        };

        private static readonly List<Subscription> subscriptions = new()
        {
            new Subscription
            {
                Id = Guid.NewGuid(),
                Name = "Netflix",
                Cost = 15.99m,
                Cycle = Cycle.Monthly,
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                NextPaymentDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1)),
                CategoryId = categories.FirstOrDefault(category => category.Name == "Entertainment").Id
            },
            new Subscription
            {
                Id = Guid.NewGuid(),
                Name = "Spotify",
                Cost = 9.99m,
                Cycle = Cycle.Monthly,
                StartDate = DateOnly.FromDateTime(DateTime.UtcNow),
                NextPaymentDate = DateOnly.FromDateTime(DateTime.UtcNow.AddMonths(1)),
                CategoryId = categories.FirstOrDefault(category => category.Name == "Music").Id
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
            return TypedResults.Ok(subscriptions.MapToSubscriptionResponses());
        }

        private static Results<NotFound, Ok<SubscriptionResponse>> GetSubscriptionById(Guid id)
        {
            var subscription = subscriptions.FirstOrDefault(s => s.Id == id);
            if(subscription == null)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(subscription.MapToSubscriptionResponse());
        }

        private static IResult CreateSubscription([FromBody] CreateSubscriptionRequest request)
        {
            var subscription = request.MapToSubscription();
            subscriptions.Add(subscription);
            return TypedResults.CreatedAtRoute(subscription.MapToSubscriptionResponse(), "GetSubscriptionById", new { id = subscription.Id });
        }

        private static Results<NotFound, NoContent> UpdateSubscription([FromBody] UpdateSubscriptionRequest updatedSubscription, Guid id)
        {
            int index = subscriptions.FindIndex(s => s.Id == id);
            if (index == -1)
            {
                return TypedResults.NotFound();
            }

            subscriptions[index] = updatedSubscription.MapToSubscription(id);

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
