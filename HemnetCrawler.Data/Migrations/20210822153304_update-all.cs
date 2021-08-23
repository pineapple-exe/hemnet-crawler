using Microsoft.EntityFrameworkCore.Migrations;

namespace HemnetCrawler.Data.Migrations
{
    public partial class updateall : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_FinalBids_FinalBidID",
                table: "Listings");

            migrationBuilder.RenameColumn(
                name: "FinalBidID",
                table: "Listings",
                newName: "FinalBidId");

            migrationBuilder.RenameIndex(
                name: "IX_Listings_FinalBidID",
                table: "Listings",
                newName: "IX_Listings_FinalBidId");

            migrationBuilder.AddForeignKey(
                name: "FK_Listings_FinalBids_FinalBidId",
                table: "Listings",
                column: "FinalBidId",
                principalTable: "FinalBids",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Listings_FinalBids_FinalBidId",
                table: "Listings");

            migrationBuilder.RenameColumn(
                name: "FinalBidId",
                table: "Listings",
                newName: "FinalBidID");

            migrationBuilder.RenameIndex(
                name: "IX_Listings_FinalBidId",
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
    }
}
