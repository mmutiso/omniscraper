using Microsoft.EntityFrameworkCore.Migrations;

namespace Omniscraper.Core.Migrations
{
    public partial class FlaggedProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "flagged",
                table: "twitter_videos",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "flagged",
                table: "twitter_videos");
        }
    }
}
