using Microsoft.EntityFrameworkCore.Migrations;

namespace HemnetCrawler.Data.Migrations
{
    public partial class AddAreaPropertiesToFinalBid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "BiArea",
                table: "FinalBids",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PropertyArea",
                table: "FinalBids",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BiArea",
                table: "FinalBids");

            migrationBuilder.DropColumn(
                name: "PropertyArea",
                table: "FinalBids");
        }
    }
}
