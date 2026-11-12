using Unit8_SRP_F24;
using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24Tests.UnitTests;

[TestClass]
public class SimpleTradeParserTests
{
    [TestMethod]
    public void ParseTrades_WhenAllLinesAreValid_MapsEachTrade()
    {
        // Arrange
        List<string> lines = new()
        {
            "AAABBB,1000,1.25",
            "CCCDDD,2500,2.50",
            "EEEFFF,5000,3.75"
        };
        var validator = new StubTradeValidator(true);
        var mapper = new StubTradeMapper();
        var parser = new SimpleTradeParser(validator, mapper);

        // Act
        List<TradeRecord> trades = parser.ParseTrades(lines).ToList();

        // Assert
        Assert.AreEqual(lines.Count, trades.Count);
        Assert.AreEqual(lines.Count, validator.ValidationCount);
        Assert.AreEqual(lines.Count, mapper.MappingCount);
        CollectionAssert.AreEqual(
            new[] { "AAA", "CCC", "EEE" },
            trades.Select(trade => trade.SourceCurrency).ToArray());
        CollectionAssert.AreEqual(
            new[] { "BBB", "DDD", "FFF" },
            trades.Select(trade => trade.DestinationCurrency).ToArray());
    }

    [TestMethod]
    public void ParseTrades_WhenLinesAreInvalid_ReturnsNoTradesAndDoesNotMap()
    {
        // Arrange
        List<string> lines = new()
        {
            "AAABBB,invalid,1.25",
            "CCCDDD,2500,not-a-price"
        };
        var validator = new StubTradeValidator(false);
        var mapper = new StubTradeMapper();
        var parser = new SimpleTradeParser(validator, mapper);

        // Act
        List<TradeRecord> trades = parser.ParseTrades(lines).ToList();

        // Assert
        Assert.AreEqual(0, trades.Count);
        Assert.AreEqual(lines.Count, validator.ValidationCount);
        Assert.AreEqual(0, mapper.MappingCount);
    }

    private sealed class StubTradeValidator(bool isValid) : ITradeValidator
    {
        public int ValidationCount { get; private set; }

        public bool ValidateTradeData(string[] fields, int currentLine)
        {
            ValidationCount++;
            return isValid;
        }
    }

    private sealed class StubTradeMapper : ITradeMapper
    {
        public int MappingCount { get; private set; }

        public TradeRecord MapTradeDataToTradeRecord(string[] fields)
        {
            MappingCount++;
            return new TradeRecord
            {
                SourceCurrency = fields[0].Substring(0, 3),
                DestinationCurrency = fields[0].Substring(3, 3)
            };
        }
    }
}