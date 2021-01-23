using Microsoft.EntityFrameworkCore.Migrations;

namespace Omniscraper.Core.Migrations
{
    public partial class unclustered_index_on_tweet_id : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_twitter_videos_tweet_id",
                table: "twitter_videos",
                column: "tweet_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_twitter_videos_tweet_id",
                table: "twitter_videos");
        }
    }
}
