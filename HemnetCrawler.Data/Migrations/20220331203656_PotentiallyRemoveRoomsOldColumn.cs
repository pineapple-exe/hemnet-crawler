using Microsoft.EntityFrameworkCore.Migrations;

namespace HemnetCrawler.Data.Migrations
{
    public partial class PotentiallyRemoveRoomsOldColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoomsOld",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "RoomsOld",
                table: "FinalBids");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoomsOld",
                table: "Listings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoomsOld",
                table: "FinalBids",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
