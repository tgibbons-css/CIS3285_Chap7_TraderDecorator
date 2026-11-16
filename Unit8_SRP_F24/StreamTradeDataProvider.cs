
using System;
using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24;

public class StreamTradeDataProvider : ITradeDataProvider
{
    private Stream stream;
    private readonly ILogger logger;

    public StreamTradeDataProvider(Stream stream, ILogger logger)
    {
        this.stream = stream;
        this.logger = logger;
    }

    public async Task<IEnumerable<string>> ReadTradeDataAsync()
    {
        logger.LogMessage("Reading trade data from file stream");

        List<string> lines = new List<string>();

        using (var reader = new StreamReader(stream))
        {
            string? line;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                lines.Add(line);
            }
        }

        return lines;
    }
}
