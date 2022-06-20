using Azure.Data.Tables;

namespace TickerSubscriptionDemo.Setup;

/// <summary>
/// Azure-related extensions for the Web Application Builder.
/// </summary>
public static class WebApplicationBuilderAzureExtensions
{
    private const string ConnectionStringKey = "CosmosTableConnectionString";
    private const string TableName = "InstrumentSnapshotTable";

    /// <summary>
    /// Adds azure table storage to the Web Application Builder.
    /// </summary>
    /// <param name="builder">The web application builder</param>
    public static void AddAzureTableStorage(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString(ConnectionStringKey);
        builder.Services.AddSingleton(new TableClient(connectionString, TableName));
    }
}