using Unit8_SRP_F24;
using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24Tests.UnitTests;

[TestClass]
public class AdjustTradeDataProviderTests
{
    [TestMethod]
    public async Task ReadTradeData_ReplacesGbpWithEur_AndPreservesOtherFields()
    {
        // Arrange
        List<string> input = new()
        {
            "GBPUSD,1000,1.25",
            "USDGBP,2500,0.75",
            "GBPGBP,3000,1.50",
            "USDJPY,5000,140.25"
        };
        List<string> expected = new()
        {
            "EURUSD,1000,1.25",
            "USDEUR,2500,0.75",
            "EUREUR,3000,1.50",
            "USDJPY,5000,140.25"
        };
        var decorator = new AdjustTradeDataProvider(new StubTradeDataProvider(input));

        // Act
        IEnumerable<string> result = await decorator.ReadTradeData();

        // Assert
        CollectionAssert.AreEqual(expected, result.ToList());
    }

    [TestMethod]
    public async Task ReadTradeData_WithNoTrades_ReturnsEmptySequence()
    {
        // Arrange
        var decorator = new AdjustTradeDataProvider(new StubTradeDataProvider(Array.Empty<string>()));

        // Act
        IEnumerable<string> result = await decorator.ReadTradeData();

        // Assert
        Assert.AreEqual(0, result.Count());
    }

    private sealed class StubTradeDataProvider(IEnumerable<string> lines) : ITradeDataProvider
    {
        public Task<IEnumerable<string>> ReadTradeData()
        {
            return Task.FromResult(lines);
        }
    }
}