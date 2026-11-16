
using System;
using System.Collections.Generic;
using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24
{
    internal class AdjustTradeDataProvider : ITradeDataProvider
    {
        private ITradeDataProvider baseProvider;

        public AdjustTradeDataProvider(ITradeDataProvider provider)
        {
            baseProvider = provider;
        }

        public async Task<IEnumerable<string>> ReadTradeDataAsync()
        {
            IEnumerable<string> trades =
                await baseProvider.ReadTradeDataAsync();

            List<string> newTrades = new List<string>();

            foreach (string trade in trades)
            {
                newTrades.Add(trade.Replace("GBP", "EUR"));
            }

            return newTrades;
        }
    }
}
