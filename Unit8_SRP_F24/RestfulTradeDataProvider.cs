using Unit8_SRP_F24.Contracts;
using System.Net.Http.Json;

namespace Unit8_SRP_F24;

public class RestfulTradeDataProvider : ITradeDataProvider
{
    private static readonly HttpClient client = new();

    private readonly string url;
    private readonly ILogger logger;

    public RestfulTradeDataProvider(string url, ILogger logger)
    {
        this.url = url;
        this.logger = logger;
    }

    public IEnumerable<string> ReadTradeData()
    {
        logger.LogMessage("Reading trade data from Restful API (This will block for now): " + url);
        Task<List<string>> task = ReadTradeDataAsync();
        task.Wait();                        // wait for the async task to complete (blocking)
        return task.Result;                 // return the result of the async task
    }

    private async Task<List<string>> ReadTradeDataAsync()
    {
        List<string>? trades = await client.GetFromJsonAsync<List<string>>(url)
            .ConfigureAwait(false);

        if (trades is null)
        {
            throw new InvalidDataException("The trade API returned no trade data.");
        }

        logger.LogMessage("Received {0} trade strings", trades.Count);
        return trades;
    }
}
