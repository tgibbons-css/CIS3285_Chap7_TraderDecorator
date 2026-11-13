using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24;

public class AdjustTradeDataProvider : ITradeDataProvider
{
    private readonly ITradeDataProvider baseProvider;

    public AdjustTradeDataProvider(ITradeDataProvider baseProvider)
    {
        this.baseProvider = baseProvider;
    }

    public async Task<IEnumerable<string>> ReadTradeDataAsync()
    {
        IEnumerable<string> trades = await baseProvider.ReadTradeDataAsync();
        return trades
            .Select(trade => trade.Replace("GBP", "EUR"));
    }
}
