using TickerSubscriptionDemo.Application.Subscriptions.Handlers;
using TickerSubscriptionDemo.Application.Subscriptions.Orchestrators;
using TickerSubscriptionDemo.Application.Subscriptions.Services;
using TickerSubscriptionDemo.Application.Subscriptions.Transformers;
using TickerSubscriptionDemo.Domain.Models;
using TickerSubscriptionDemo.Infrastructure.Deribit.Services;
using TickerSubscriptionDemo.Repositories;
using TickerSubscriptionDemo.Repositories.Models;
using TickerSubscriptionDemo.Repositories.Transformers;
using TickerSubscriptionDemo.Services;
using TickerSubscriptionDemo.Services.Contracts;
using TickerSubscriptionDemo.Setup;
using TickerSubscriptionDemo.Utilities.Factories;

var builder = WebApplication.CreateBuilder(args);

builder.AddAzureTableStorage();

builder.AddConfigurationSettings();

builder.Services.AddHostedService<SubscriptionBackgroundService>();

builder.Services.AddSingleton<ISubscriptionOrchestrator, SubscriptionOrchestrator>();

builder.Services.AddSingleton<IJsonRpcService, JsonRpcService>();
builder.Services.AddSingleton<IDeribitRpcClientService, DeribitRpcClientService>();
builder.Services.AddSingleton<IDataService, DeribitService>();

builder.Services.AddSingleton<IConnectionCheckHandlerFactory, ConnectionCheckHandlerFactory>();

builder.Services.AddSingleton<ITableClientFacade<InstrumentSubscriptionEntity>, TableClientFacade<InstrumentSubscriptionEntity>>();
builder.Services.AddSingleton<IRepository<InstrumentSubscriptionSnapshot>, TableRepository<InstrumentSubscriptionSnapshot, InstrumentSubscriptionEntity>>();
builder.Services.AddSingleton<ITableEntityTransformer<InstrumentSubscriptionSnapshot, InstrumentSubscriptionEntity>, InstrumentTableEntityTransformer>();

builder.Services.AddSingleton<ISubscriptionRequestTransformer<Instrument>, InstrumentSubscriptionRequestTransformer>();
builder.Services.AddSingleton<ISubscriptionResponseTransformer<InstrumentSubscriptionSnapshot>, InstrumentSubscriptionResponseTransformer>();
builder.Services.AddSingleton<ISubscriptionHandler, PersistingSubscriptionHandler<InstrumentSubscriptionSnapshot>>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
