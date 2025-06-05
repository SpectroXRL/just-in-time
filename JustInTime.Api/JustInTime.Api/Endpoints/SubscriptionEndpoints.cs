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

        private static IResult GetAllSubscriptions(string? categoryName,
            string? orderOption, string? sortOrder)
        {
            SubscriptionOrderOptions orderOpt = SubscriptionOrderOptions.Name;
            if (orderOption != null)
            {
                if (!Enum.TryParse<SubscriptionOrderOptions>(orderOption, true, out orderOpt))
                {
                    return TypedResults.BadRequest();
                }
            };

            if (sortOrder != null && !string.Equals(sortOrder, "asc", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase))
            {
                return TypedResults.BadRequest();
            }

            Guid categoryID = string.IsNullOrEmpty(categoryName) 
                ? Guid.Empty 
                : categories.FirstOrDefault(category => category.Name.Equals(categoryName, StringComparison.OrdinalIgnoreCase)).Id;

            var subscriptions = string.IsNullOrEmpty(categoryName)
                ? SubscriptionEndpoints.subscriptions
                : SubscriptionEndpoints.subscriptions.Where(s => s.CategoryId == categoryID).ToList();

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
