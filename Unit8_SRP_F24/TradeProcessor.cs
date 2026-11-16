
using Microsoft.Data.SqlClient;
using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24;

public class TradeProcessor
{
    private readonly ITradeDataProvider tradeDataProvider;
    private readonly ITradeParser tradeParser;
    private readonly ITradeStorage tradeStorage;

    public TradeProcessor(ITradeDataProvider tradeDataProvider, ITradeParser tradeParser, ITradeStorage tradeStorage)
    {
        this.tradeDataProvider = tradeDataProvider;
        this.tradeParser = tradeParser;
        this.tradeStorage = tradeStorage;
    }

    public async Task ProcessTrades()
    {
        var lines = await tradeDataProvider.ReadTradeDataAsync();
        var trades = tradeParser.ParseTrades(lines);
        tradeStorage.StoreTrades(trades);
    }
}
