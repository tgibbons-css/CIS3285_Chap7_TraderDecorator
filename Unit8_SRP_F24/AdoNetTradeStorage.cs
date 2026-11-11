using Microsoft.Data.SqlClient;
using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24;

public class AdoNetTradeStorage : ITradeStorage
{

    ILogger logger = new ConsoleLogger();
    public AdoNetTradeStorage(ILogger logger)
    {
        this.logger = logger;
    }

    /// <summary>
    /// Write the trade records to the database
    /// </summary>
    /// <param name="trades"> A list of TradeRecord objects </param>
    public void StoreTrades(IEnumerable<TradeRecord> trades)
    {
        logger.LogMessage("INFO: Connecting to database");
        // Template for connection string from database connection file
        //    The @ sign allows for back slashes
        //    Watch for double quotes which must be escaped using "" 
        //    Watch for extra spaces after C: and avoid paths with - hyphens -
        string genericConnectString = @"Data Source=(local);Initial Catalog=TradeDatabase;Integrated Security=True;";
        // The datadirConnectString connection string uses |DataDirectory| 
        //    and assumes the tradedatabase.mdf file is stored in 
        //    SingleResponsibilityPrinciple\bin\Debug 
        string datadirConnectString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\tradedatabase.mdf;Integrated Security=True;Connect Timeout=30;";
        // This users the Azure connection string
        string azureConnectString = @"Server=tcp:cis3285-sql-server.database.windows.net,1433; Initial Catalog = Unit8_TradesDatabase; Persist Security Info=False; User ID=cis3285;Password=Saints2026SQL; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 60;";
        string tomConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\tgibbons\Documents\tradedatabase.mdf;Integrated Security=True;Connect Timeout=30";
        // Change the connection string used to match the one you want
        using (var connection = new SqlConnection(azureConnectString))
        {
            logger.LogMessage("INFO: Going to open database connection");

            try
            {
                connection.Open();
            }
            // Azure returns error 40613 (database unavailable) if the databse is asleep.  This catch block will log a message and rethrow the exception so the program can be restarted.
            catch (SqlException ex) when (ex.Number == 40613)
            {
                logger.LogMessage("ERROR: The Azure database is waking up. Please wait a moment, then try running the program again.");
                throw;
            }

            logger.LogMessage("INFO: Database connection OPEN");

            using (var transaction = connection.BeginTransaction())
            {
                foreach (var trade in trades)
                {
                    var command = connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.CommandText = "dbo.insert_trade";
                    command.Parameters.AddWithValue("@sourceCurrency", trade.SourceCurrency);
                    command.Parameters.AddWithValue("@destinationCurrency", trade.DestinationCurrency);
                    command.Parameters.AddWithValue("@lots", trade.Lots);
                    command.Parameters.AddWithValue("@price", trade.Price);
                    logger.LogMessage(
                        "INFO: Adding trade to database: Source={0}, Destination={1}, Lots={2}, Price={3}",
                        trade.SourceCurrency,
                        trade.DestinationCurrency,
                        trade.Lots,
                        trade.Price);

                    command.ExecuteNonQuery();
                }

                transaction.Commit();
            }
            connection.Close();
        }

        logger.LogMessage("INFO: {0} trades processed", trades.Count());
    }
}
