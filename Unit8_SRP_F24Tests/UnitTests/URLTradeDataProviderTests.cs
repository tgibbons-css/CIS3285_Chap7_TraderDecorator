
using System;
using System.Reflection;
using Unit8_SRP_F24;
using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24Tests.UnitTests;

[TestClass()]
public class URLTradeDataProviderTests
{
    private int countStrings(IEnumerable<string> collectionOfStrings)
    {
        int count = 0;

        foreach (var trade in collectionOfStrings)
        {
            count++;
        }

        return count;
    }

    [TestMethod()]
    public async Task TestSize1()
    {
        // Arrange
        ILogger logger = new ConsoleLogger();

        string tradeURL = "https://raw.githubusercontent.com/tgibbons-css/CIS3285_CurrencyTraderWeb/refs/heads/master/CurrencyTraderTests/trades_1good.txt";

        ITradeDataProvider tradeProvider = new URLTradeDataProvider(tradeURL, logger);

        // Act
        IEnumerable<string> trades = await tradeProvider.ReadTradeDataAsync();

        // Assert
        Assert.AreEqual(1, countStrings(trades));
    }

    [TestMethod()]
    public async Task TestSize5()
    {
        // Arrange
        ILogger logger = new ConsoleLogger();

        string tradeURL = "https://raw.githubusercontent.com/tgibbons-css/CIS3285_CurrencyTraderWeb/refs/heads/master/CurrencyTraderTests/trades_5good.txt";

        ITradeDataProvider tradeProvider = new URLTradeDataProvider(tradeURL, logger);

        // Act
        IEnumerable<string> trades = await tradeProvider.ReadTradeDataAsync();

        // Assert
        Assert.AreEqual(5, countStrings(trades));
    }
}
