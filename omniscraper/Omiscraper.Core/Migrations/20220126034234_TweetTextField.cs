using Microsoft.EntityFrameworkCore.Migrations;

namespace Omniscraper.Core.Migrations
{
    public partial class TweetTextField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "text",
                table: "twitter_videos",
                type: "character varying(290)",
                maxLength: 290,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "text",
                table: "twitter_videos");
        }
    }
}
