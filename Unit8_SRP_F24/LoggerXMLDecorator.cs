using System;
using Unit8_SRP_F24.Contracts;

namespace Unit8_SRP_F24;

public class LoggerXMLDecorator : ILogger
{
    private readonly ILogger _baseLogger;

    public LoggerXMLDecorator(ILogger logger)
    {
        _baseLogger = logger;
    }

    public void LogMessage(string message, params object[] args)
    {

        using (StreamWriter logfile = File.AppendText("log.xml"))
        {
            string xmlMessage = $"<log><message>{string.Format(message, args)}</message></log>";
            logfile.WriteLine(xmlMessage);
        }
        _baseLogger.LogMessage(message, args);


    }
}
