using Microsoft.EntityFrameworkCore.Migrations;

namespace HemnetCrawler.Data.Migrations
{
    public partial class ChangeFinalBidHrefToSimplyHref : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinalBidHref",
                table: "Listings");

            migrationBuilder.AddColumn<string>(
                name: "Href",
                table: "Listings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Href",
                table: "Listings");

            migrationBuilder.AddColumn<string>(
                name: "FinalBidHref",
                table: "Listings",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
