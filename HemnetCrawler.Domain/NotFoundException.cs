using System;

namespace HemnetCrawler.Domain
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entityType) : base($"{ entityType} not found.")
        {

        }
    }
}
