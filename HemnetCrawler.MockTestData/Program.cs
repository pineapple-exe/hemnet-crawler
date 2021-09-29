using HemnetCrawler.Data;
using HemnetCrawler.Data.Repositories;
using HemnetCrawler.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace HemnetCrawler.MockTestData
{
    class Program
    {
        static void Main(string[] args)
        {
            HemnetCrawlerDbContext context = new();

            CreateTestDatabaseWithSchema(context);
            CreateMockData(context);
        }

        static void CreateTestDatabaseWithSchema(HemnetCrawlerDbContext context)
        {
            string sql = "CREATE DATABASE HemnetCrawlerTest";

            using SqlConnection connection = new("Server=localhost;Database=master;Trusted_Connection=True;");

            SqlCommand command = new(sql, connection);

            command.Connection.Open();
            command.ExecuteNonQuery();

            context.Database.Migrate();
        }

        static void CreateMockData(HemnetCrawlerDbContext context)
        {
            FinalBidRepository finalBidRepository = new(context);
            ListingRepository listingRepository = new(context);

            listingRepository.AddListing(new Listing()
            {
                Id = 1,
                FinalBidId = 100
            });

            finalBidRepository.AddFinalBid(new FinalBid()
            {
                Id = 100
            });
        }
    }
}
