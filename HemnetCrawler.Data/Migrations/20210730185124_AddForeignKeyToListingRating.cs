using Microsoft.EntityFrameworkCore.Migrations;

namespace HemnetCrawler.Data.Migrations
{
    public partial class AddForeignKeyToListingRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Listings_ListingID",
                table: "Images");

            migrationBuilder.RenameColumn(
                name: "ListingID",
                table: "Images",
                newName: "ListingId");

            migrationBuilder.RenameIndex(
                name: "IX_Images_ListingID",
                table: "Images",
                newName: "IX_Images_ListingId");

            migrationBuilder.CreateIndex(
                name: "IX_ListingRatings_ListingId",
                table: "ListingRatings",
                column: "ListingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Listings_ListingId",
                table: "Images",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ListingRatings_Listings_ListingId",
                table: "ListingRatings",
                column: "ListingId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Listings_ListingId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_ListingRatings_Listings_ListingId",
                table: "ListingRatings");

            migrationBuilder.DropIndex(
                name: "IX_ListingRatings_ListingId",
                table: "ListingRatings");

            migrationBuilder.RenameColumn(
                name: "ListingId",
                table: "Images",
                newName: "ListingID");

            migrationBuilder.RenameIndex(
                name: "IX_Images_ListingId",
                table: "Images",
                newName: "IX_Images_ListingID");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Listings_ListingID",
                table: "Images",
                column: "ListingID",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
