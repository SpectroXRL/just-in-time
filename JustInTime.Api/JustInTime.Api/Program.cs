using JustInTime.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.RegisterSubscriptionEndpoitns();

app.Run();
