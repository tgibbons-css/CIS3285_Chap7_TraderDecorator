
using Unit8_SRP_F24;
using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24Tests.UnitTests
{
    [TestClass]
    public class SimpleTradeParserTests
    {
        [TestMethod]
        public void TestValidTrades()
        {
            // Arrange
            string[] lines =
            {
                "GBPUSD,1000,1.51",
                "EURUSD,2000,1.25"
            };

            ITradeValidator validator = new PositiveValidator();
            ITradeMapper mapper = new DummyMapper();

            SimpleTradeParser parser =
                new SimpleTradeParser(validator, mapper);

            // Act
            var trades = parser.ParseTrades(lines).ToList();

            // Assert
            Assert.AreEqual(2, trades.Count);
        }

        [TestMethod]
        public void TestInvalidTrades()
        {
            // Arrange
            string[] lines =
            {
                "GBPUSD,1000,1.51",
                "EURUSD,2000,1.25"
            };

            ITradeValidator validator = new NegativeValidator();
            ITradeMapper mapper = new DummyMapper();

            SimpleTradeParser parser =
                new SimpleTradeParser(validator, mapper);

            // Act
            var trades = parser.ParseTrades(lines).ToList();

            // Assert
            Assert.AreEqual(0, trades.Count);
        }

        // Always accepts trades
        private class PositiveValidator : ITradeValidator
        {
            public bool ValidateTradeData(
                string[] fields, int lineCount)
            {
                return true;
            }
        }

        // Always rejects trades
        private class NegativeValidator : ITradeValidator
        {
            public bool ValidateTradeData(
                string[] fields, int lineCount)
            {
                return false;
            }
        }

        // Creates a sample trade record
        private class DummyMapper : ITradeMapper
        {
            public TradeRecord MapTradeDataToTradeRecord(
                string[] fields)
            {
                TradeRecord trade = new TradeRecord();

                trade.SourceCurrency = "GBP";
                trade.DestinationCurrency = "USD";
                trade.Lots = 1.0f;
                trade.Price = 1.51m;

                return trade;
            }
        }
    }
}
