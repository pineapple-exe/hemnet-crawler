using Microsoft.EntityFrameworkCore.Migrations;

namespace HemnetCrawler.Data.Migrations
{
    public partial class AddPostalCodeColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PostalCode",
                table: "Listings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PostalCode",
                table: "FinalBids",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "FinalBids");
        }
    }
}
