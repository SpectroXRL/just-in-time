using JustInTime.Api.Contracts.Requests;
using JustInTime.Api.Contracts.Response;
using JustInTime.Api.Mappings;
using JustInTime.Api.Models;
using JustInTime.Api.Services.Categories;
using JustInTime.Api.Services.Subscriptions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace JustInTime.Api.Endpoints
{
    public static class SubscriptionEndpoints
    {

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

        private static async Task<IResult> GetAllSubscriptions([FromServices]ISubscriptionService subscriptionService,
            [FromServices]ICategoryService categoryService,
            string? categoryName, string? orderOption, string? sortOrder)
        {
            var categories = await categoryService.GetCategoriesAsync();
            //Add category check
            var categoryId = await categoryService.GetCategoryId(categoryName);
            var (subscriptions, isValidRequest) = await subscriptionService.GetSubscriptionsAsync(categoryId, orderOption, sortOrder);

            if(!isValidRequest)
            {
                return TypedResults.BadRequest();
            }

            return TypedResults.Ok(subscriptions.MapToSubscriptionResponses(categories));
        }

        private static async Task<Results<NotFound, Ok<SubscriptionResponse>>> GetSubscriptionById([FromServices]ISubscriptionService subscriptionService, 
            [FromServices]ICategoryService categoryService,  Guid id)
        {
            var categories = await categoryService.GetCategoriesAsync();
            var (subscription, isValidRequest) = await subscriptionService.GetSubscriptionAsync(id);

            if (!isValidRequest)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.Ok(subscription.MapToSubscriptionResponse(categories));
        }

        private static async Task<IResult> CreateSubscription([FromServices] ISubscriptionService subscriptionService,
            [FromServices]ICategoryService categoryService,
            [FromBody] CreateSubscriptionRequest request)
        {
            var categories = await categoryService.GetCategoriesAsync();
            var (subscription, isValidRequest) = await subscriptionService.CreateSubscriptionAsync(request.MapToSubscription(categories));

            return TypedResults.CreatedAtRoute(subscription.MapToSubscriptionResponse(categories), "GetSubscriptionById", new { id = subscription.Id });
        }

        private static async Task<Results<NotFound, NoContent>> UpdateSubscription([FromServices]ISubscriptionService subscriptionService, 
            [FromServices]ICategoryService categoryService, [FromBody] UpdateSubscriptionRequest updatedSubscription, Guid id)
        {
            var categories = await categoryService.GetCategoriesAsync();
            var result = await subscriptionService.UpdateSubscriptionAsync(id, updatedSubscription.MapToSubscription(id, categories));

            if (!result)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.NoContent();
        }

        private static async Task<Results<NotFound, NoContent>> DeleteSubscription([FromServices]ISubscriptionService subscriptionService,
            Guid id)
        {
            var result = await subscriptionService.DeleteSubscriptionAsync(id);

            if (!result)
            {
                return TypedResults.NotFound();
            }

            return TypedResults.NoContent();
        }

    }
}
