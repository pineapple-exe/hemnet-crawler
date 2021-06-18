using System;
using System.Collections.Generic;
using System.Text;

namespace HemnetCrawler.Domain
{
    public class ConsoleLogger : ILogger
    {
        public ConsoleLogger()
        {

        }

        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
