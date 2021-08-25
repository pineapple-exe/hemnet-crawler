using System;
using System.Text.RegularExpressions;

namespace HemnetCrawler.ConsoleApp
{
    internal class Utils
    {
        public static double GetTotalDays(DateTimeOffset from, DateTimeOffset to)
        {
            double secondsDiff = (to - from).TotalSeconds;
            int secondsPerDay = 24 * 60 * 60;
            return secondsDiff / secondsPerDay;
        }

        public static int DigitPurist(string impure)
        {
            Regex nonDigitPattern = new("\\D+");
            return int.Parse(nonDigitPattern.Replace(impure, ""));
        }
    }
}
