using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24;

public class AdjustTradeDataProvider : ITradeDataProvider
{
    private readonly ITradeDataProvider _baseProvider;

    public AdjustTradeDataProvider(ITradeDataProvider baseProvider)
    {
        _baseProvider = baseProvider;
    }
    
    public async Task<IEnumerable<string>> ReadTradeData()
    {
        IEnumerable<string> tradeData = await _baseProvider.ReadTradeData();
        List<string> adjustedDataList = new List<string>();

        foreach (string trade in tradeData)
        {
            adjustedDataList.Add(trade.Replace("GBP", "EUR"));
        }
        return adjustedDataList;
    }
}
