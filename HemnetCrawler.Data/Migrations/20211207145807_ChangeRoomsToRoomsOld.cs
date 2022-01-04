using Microsoft.EntityFrameworkCore.Migrations;

namespace HemnetCrawler.Data.Migrations
{
    public partial class ChangeRoomsToRoomsOld : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Rooms",
                table: "Listings",
                newName: "RoomsOld");

            migrationBuilder.RenameColumn(
                name: "Rooms",
                table: "FinalBids",
                newName: "RoomsOld");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RoomsOld",
                table: "Listings",
                newName: "Rooms");

            migrationBuilder.RenameColumn(
                name: "RoomsOld",
                table: "FinalBids",
                newName: "Rooms");
        }
    }
}
