using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HemnetCrawler.Data.Migrations
{
    public partial class AddFinalBidTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinalBids",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    SoldDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DemandedPrice = table.Column<int>(type: "int", nullable: false),
                    PriceDevelopment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PricePerSquareMeter = table.Column<int>(type: "int", nullable: true),
                    HomeType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnershipType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rooms = table.Column<int>(type: "int", nullable: true),
                    LivingArea = table.Column<double>(type: "float", nullable: true),
                    Fee = table.Column<int>(type: "int", nullable: true),
                    ConstructionYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LandLeaseFee = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinalBids", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinalBids");
        }
    }
}
