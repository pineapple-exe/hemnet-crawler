using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HemnetCrawler.Data.Migrations
{
    public partial class ChangeDaysOnHemnetToPublished : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysOnHemnet",
                table: "Listings");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "Published",
                table: "Listings",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Published",
                table: "Listings");

            migrationBuilder.AddColumn<int>(
                name: "DaysOnHemnet",
                table: "Listings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
