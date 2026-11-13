using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24;

public class URLTradeDataProvider : ITradeDataProvider
{
    private readonly string url;        // URL to fetch trade data from
    private readonly ILogger logger;    //  Logger to log messages

    public URLTradeDataProvider(string url, ILogger logger)
    {
        this.url = url;
        this.logger = logger;
    }

    public async Task<IEnumerable<string>> ReadTradeDataAsync()
    {
        logger.LogMessage("Reading trade data from URL: " + url);
        List<string> tradeData = new List<string>();
        using (HttpClient client = new HttpClient())
        {
            using HttpResponseMessage response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                // log error and throw an exception if the URL fails
                logger.LogMessage("Error fetching trade data from URL: " + url + ". Status code: " + response.StatusCode);
            }
            using (Stream stream = await response.Content.ReadAsStreamAsync())
            using (StreamReader reader = new StreamReader(stream))
            {
                string? line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    tradeData.Add(line);
                }
            }
        }
        return tradeData;
    }
}
