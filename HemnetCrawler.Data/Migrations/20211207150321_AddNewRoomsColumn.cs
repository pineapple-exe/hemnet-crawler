using Microsoft.EntityFrameworkCore.Migrations;

namespace HemnetCrawler.Data.Migrations
{
    public partial class AddNewRoomsColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Rooms",
                table: "Listings",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Rooms",
                table: "FinalBids",
                type: "float",
                nullable: true);

            migrationBuilder.Sql("UPDATE dbo.Listings " +
                                 "SET Rooms = convert(float, replace(replace(RoomsOld, ' rum', ''), ',', '.')) ");

            migrationBuilder.Sql("UPDATE dbo.FinalBids " +
                                 "SET Rooms = convert(float, replace(replace(RoomsOld, ' rum', ''), ',', '.')) ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rooms",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "Rooms",
                table: "FinalBids");
        }
    }
}
