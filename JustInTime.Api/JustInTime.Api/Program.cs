using JustInTime.Api.Endpoints;
using JustInTime.Api.Repositories.Categories;
using JustInTime.Api.Repositories.Subscriptions;
using JustInTime.Api.Services.Categories;
using JustInTime.Api.Services.Subscriptions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ISubscriptionRepository, InMemSubscriptionRepository>();
builder.Services.AddScoped<ICategoryRepository, InMemCategoryRepository>();

var app = builder.Build();

app.RegisterSubscriptionEndpoitns();

app.Run();
