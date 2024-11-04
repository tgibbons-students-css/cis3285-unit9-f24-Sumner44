using System;
using System.IO;
using SingleResponsibilityPrinciple.Contracts;

namespace SingleResponsibilityPrinciple
{
    public class ConsoleLogger : ILogger
    {
        private const string LogFilePath = "log.xml";

        public void LogWarning(string message, params object[] args)
        {
            LogMessage("WARN", message, args);
        }

        public void LogInfo(string message, params object[] args)
        {
            LogMessage("INFO", message, args);
        }

        private void LogMessage(string type, string message, params object[] args)
        {
            // Format the message for console output
            string formattedMessage = string.Format(message, args);
            Console.WriteLine($"{type}: {formattedMessage}");

            // Append log to XML file
            using (StreamWriter logfile = File.AppendText(LogFilePath))
            {
                logfile.WriteLine($"<log><type>{type}</type><message>{formattedMessage}</message></log>");
            }
        }
    }
}
