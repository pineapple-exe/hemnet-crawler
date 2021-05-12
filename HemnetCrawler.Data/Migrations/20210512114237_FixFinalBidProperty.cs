using Microsoft.EntityFrameworkCore.Migrations;

namespace HemnetCrawler.Data.Migrations
{
    public partial class FixFinalBidProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_FinalBids_FinalBidIDId",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "FinalBid",
                table: "Listings");

            migrationBuilder.RenameColumn(
                name: "FinalBidIDId",
                table: "Listings",
                newName: "FinalBidID");

            migrationBuilder.RenameIndex(
                name: "IX_Listings_FinalBidIDId",
                table: "Listings",
                newName: "IX_Listings_FinalBidID");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_FinalBids_FinalBidID",
                table: "Listings",
                column: "FinalBidID",
                principalTable: "FinalBids",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_FinalBids_FinalBidID",
                table: "Listings");

            migrationBuilder.RenameColumn(
                name: "FinalBidID",
                table: "Listings",
                newName: "FinalBidIDId");

            migrationBuilder.RenameIndex(
                name: "IX_Listings_FinalBidID",
                table: "Listings",
                newName: "IX_Listings_FinalBidIDId");

            migrationBuilder.AddColumn<int>(
                name: "FinalBid",
                table: "Listings",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_FinalBids_FinalBidIDId",
                table: "Listings",
                column: "FinalBidIDId",
                principalTable: "FinalBids",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
