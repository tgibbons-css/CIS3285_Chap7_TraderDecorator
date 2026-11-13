using System;
using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24;

public class AdjustTradeDataProvider : ITradeDataProvider
{
    private readonly ITradeDataProvider _baseTradeDataProvider;

    public AdjustTradeDataProvider(ITradeDataProvider baseTradeDataProvider)
    {
        _baseTradeDataProvider = baseTradeDataProvider;
    }
    public async Task<IEnumerable<string>> ReadTradeData()
    {
        List<string> tradeData = (await _baseTradeDataProvider.ReadTradeData()).ToList();

        for (int i = 0; i < tradeData.Count; i++)
        {
            if (tradeData[i].Contains("GBP"))
            {
                tradeData[i] = tradeData[i].Replace("GBP", "EUR");
            }
        }
        return tradeData;
    }
}
