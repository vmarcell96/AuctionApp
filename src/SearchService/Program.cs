using System.Net;
using MassTransit;
using Polly;
using Polly.Extensions.Http;
using SearchService.Consumers;
using SearchService.Data;
using SearchService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddAutoMapper(_ => { }, AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHttpClient<AuctionServiceHttpClient>().AddPolicyHandler(GetPolicy());
builder.Services.AddMassTransit(x =>
{
    // any other consumer created in the same namespace will be automatically registered
    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();

    // kebabcase search-Auction-Created
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ReceiveEndpoint("search-auction-created", e =>
        {
            // 5 retries, every 5 seconds
            e.UseMessageRetry(r => r.Interval(5, 5));

            e.ConfigureConsumer<AuctionCreatedConsumer>(context);
        });

        cfg.ReceiveEndpoint("search-auction-updated", e =>
        {
            // 5 retries, every 5 seconds
            e.UseMessageRetry(r => r.Interval(5, 5));

            e.ConfigureConsumer<AuctionUpdatedConsumer>(context);
        });

        cfg.ReceiveEndpoint("search-auction-deleted", e =>
        {
            // 5 retries, every 5 seconds
            e.UseMessageRetry(r => r.Interval(5, 5));

            e.ConfigureConsumer<AuctionDeletedConsumer>(context);
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

// this way the search service will run when the auctions service is not available
app.Lifetime.ApplicationStarted.Register(async () =>
{
    try
    {
        await DbInitializer.InitDb(app);
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
    }
});

app.Run();

// if auctionService is down, we handle the exception and keep trying
static IAsyncPolicy<HttpResponseMessage> GetPolicy()
            => HttpPolicyExtensions.HandleTransientHttpError()
                // with this line we will keep trying even if the response is notfound
                .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
                // keep on trying every 3 seconds
                .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3));
