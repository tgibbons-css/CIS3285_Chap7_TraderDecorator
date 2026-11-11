using System.Reflection;
using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24;


class Program
{
    static void Main(string[] args)
    {
        ILogger consoleLogger = new ConsoleLogger();
        LoggerXMLDecorator logger = new LoggerXMLDecorator(consoleLogger);  // decorate the console logger with XML logging functionality

        // get the local, embedded trades.txt file
        var tradeStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Unit8_SRP_F24.trades.txt");
        if (tradeStream is null)
        {
            logger.LogMessage("ERROR: Could not find the embedded trades.txt file.");
            return;
        }

        // URL to read trade file from
        string tradeURL = "http://raw.githubusercontent.com/tgibbons-css/CIS3285_Unit9_F24/refs/heads/master/SingleResponsibilityPrinciple/trades.txt";

        //URL for Restful API
        string restfulURL = "http://unit9trader.azurewebsites.net/api/TradeData";


        // set up different trade data providers
        ITradeDataProvider fileProvider = new StreamTradeDataProvider(tradeStream, logger);
        ITradeDataProvider URLProvider = new URLTradeDataProvider(tradeURL, logger);
        ITradeDataProvider RestfulAPIProvider = new RestfulTradeDataProvider(restfulURL, logger);

        ITradeMapper tradeMapper = new SimpleTradeMapper();
        ITradeValidator tradeValidator = new SimpleTradeValidator(logger);
        ITradeParser tradeParser = new SimpleTradeParser(tradeValidator, tradeMapper);
        ITradeStorage tradeStorage = new AdoNetTradeStorage(logger);

        // set up the trade processor with different trade data providers
        //var tradeProcessor = new TradeProcessor(fileProvider, tradeParser, tradeStorage);
        //var tradeProcessor = new TradeProcessor(URLProvider, tradeParser, tradeStorage);
        var tradeProcessor = new TradeProcessor(RestfulAPIProvider, tradeParser, tradeStorage);

        tradeProcessor.ProcessTrades();

    }
}
