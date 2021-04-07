using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HemnetCrawler.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Listings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewConstruction = table.Column<bool>(type: "bit", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: false),
                    City = table.Column<string>(type: "nvarchar(96)", maxLength: 96, nullable: false),
                    Price = table.Column<int>(type: "int", nullable: true),
                    PricePerSquareMeter = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HomeType = table.Column<string>(type: "nvarchar(22)", maxLength: 22, nullable: false),
                    OwnershipType = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                    Rooms = table.Column<int>(type: "int", nullable: true),
                    Balcony = table.Column<bool>(type: "bit", nullable: false),
                    Floor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LivingArea = table.Column<double>(type: "float", nullable: true),
                    BiArea = table.Column<double>(type: "float", nullable: true),
                    PropertyArea = table.Column<int>(type: "int", nullable: false),
                    Fee = table.Column<int>(type: "int", nullable: true),
                    ConstructionYear = table.Column<int>(type: "int", nullable: true),
                    HomeOwnersAssociation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Utilities = table.Column<int>(type: "int", nullable: true),
                    EnergyClassification = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    Visits = table.Column<int>(type: "int", nullable: false),
                    DaysOnHemnet = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Listings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ListingID = table.Column<int>(type: "int", nullable: false),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Listings_ListingID",
                        column: x => x.ListingID,
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_ListingID",
                table: "Images",
                column: "ListingID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Listings");
        }
    }
}
