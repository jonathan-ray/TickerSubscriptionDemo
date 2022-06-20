using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using TickerSubscriptionDemo.Domain.Models;
using TickerSubscriptionDemo.Repositories;
using TickerSubscriptionDemo.Repositories.Queries;

namespace TickerSubscriptionDemo.Controllers;

/// <summary>
/// Controller for accessing instrument-based data.
/// </summary>
[ApiController]
[Route("[controller]")]
public class InstrumentsController : ControllerBase
{
    private readonly IRepository<InstrumentSubscriptionSnapshot> repository;

    public InstrumentsController(IRepository<InstrumentSubscriptionSnapshot> repository)
    {
        this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    /// <summary>
    /// Gets all snapshots for a specific instrument.
    /// </summary>
    /// <param name="instrumentName">The name of the instrument.</param>
    /// <returns>All snapshot data.</returns>
    [HttpGet("/instruments/{instrumentName}/snapshots")]
    public async Task<IEnumerable<JsonNode?>> GetInstrumentSnapshots(string instrumentName)
    {
        if (string.IsNullOrWhiteSpace(instrumentName))
        {
            throw new ArgumentNullException(nameof(instrumentName));
        }

        var snapshots = await this.repository.Query(
            new TableRepositoryQueryBuilder()
                .WithPartitionKeyEqualTo(instrumentName)
                .Build());

        return snapshots.Select(snapshot => JsonNode.Parse(snapshot.Data));
    }
}