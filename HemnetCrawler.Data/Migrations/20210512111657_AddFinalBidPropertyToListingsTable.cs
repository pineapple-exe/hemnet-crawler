using Microsoft.EntityFrameworkCore.Migrations;

namespace HemnetCrawler.Data.Migrations
{
    public partial class AddFinalBidPropertyToListingsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FinalBid",
                table: "Listings",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FinalBidIDId",
                table: "Listings",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Listings_FinalBidIDId",
                table: "Listings",
                column: "FinalBidIDId");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_FinalBids_FinalBidIDId",
                table: "Listings",
                column: "FinalBidIDId",
                principalTable: "FinalBids",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_FinalBids_FinalBidIDId",
                table: "Listings");

            migrationBuilder.DropIndex(
                name: "IX_Listings_FinalBidIDId",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "FinalBid",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "FinalBidIDId",
                table: "Listings");
        }
    }
}
