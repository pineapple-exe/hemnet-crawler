using HemnetCrawler.Data;
using HemnetCrawler.Data.Repositories;
using HemnetCrawler.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;

namespace HemnetCrawler.MockTestData
{
    class Program
    {
        static void Main(string[] args)
        {
            string databaseName = "HemnetCrawlerTest";
            DeleteDatabase(databaseName);
            CreateDatabase(databaseName);

            HemnetCrawlerDbContext context = new($"Server=localhost;Database={databaseName};Trusted_Connection=True;");
            context.Database.Migrate();

            CreateMockData(context);
        }

        static void DeleteDatabase(string databaseName)
        {
            string sql = $"DROP DATABASE IF EXISTS {databaseName}";

            using SqlConnection connection = new("Server=localhost;Database=master;Trusted_Connection=True;");

            SqlCommand command = new(sql, connection);

            command.Connection.Open();
            command.ExecuteNonQuery();
        }

        static void CreateDatabase(string databaseName)
        {
            string sql = $"CREATE DATABASE {databaseName}";

            using SqlConnection connection = new("Server=localhost;Database=master;Trusted_Connection=True;");

            SqlCommand command = new(sql, connection);

            command.Connection.Open();
            command.ExecuteNonQuery();
        }

        static void CreateMockData(HemnetCrawlerDbContext context)
        {
            FinalBidRepository finalBidRepository = new(context);
            ListingRepository listingRepository = new(context);

            finalBidRepository.AddFinalBid(new FinalBid()
            { 
                HemnetId = 100,
                Href = "https://www.hemnet.se/salda/lagenhet-3rum-goteborgs-kommun-testvagen-1-b-100",
                LastUpdated = DateTimeOffset.Now,
                Street = "Testvägen 1 B",
                City = "Teststaden",
                Price = 200,
                SoldDate = new DateTime(2021, 10, 4),
                DemandedPrice = 100,
                PriceDevelopment = "+100 kr(+100%)",
                HomeType = "Lägenhet",
                OwnershipType = "Bostadsrätt",
                Rooms = "3 rum",
                PropertyArea = 75,
                ConstructionYear = "2021"
            });

            listingRepository.AddListing(new Listing() 
            {
                HemnetId = 100,
                Href = "https://www.hemnet.se/bostad/lagenhet-3rum-goteborgs-kommun-testvagen-1-b-100",
                LastUpdated = DateTimeOffset.Now.AddDays(-10),
                NewConstruction = true,
                Street = "Testvägen 1 B",
                City = "Teststaden",
                Description = "Nybyggt hem i ett lugnt men centralt område.",
                HomeType = "Lägenhet",
                OwnershipType = "Bostadsrätt",
                Rooms = "3 rum",
                Balcony = false,
                Floor = "Andra våningen",
                PropertyArea = 75,
                ConstructionYear = "2021",
                HomeOwnersAssociation = "Bostadsföreningen med stort B!",
                EnergyClassification = "C",
                Visits = 32,
                Published = DateTime.Now.AddDays(-20),
                FinalBidId = finalBidRepository.GetAll().First().Id 
            });
        }
    }
}
