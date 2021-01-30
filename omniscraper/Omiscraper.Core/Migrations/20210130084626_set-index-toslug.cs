using Microsoft.EntityFrameworkCore.Migrations;

namespace Omniscraper.Core.Migrations
{
    public partial class setindextoslug : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_twitter_videos_tweet_with_video_id",
                table: "twitter_videos");

            migrationBuilder.CreateIndex(
                name: "ix_twitter_videos_slug",
                table: "twitter_videos",
                column: "slug",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_twitter_videos_slug",
                table: "twitter_videos");

            migrationBuilder.CreateIndex(
                name: "ix_twitter_videos_tweet_with_video_id",
                table: "twitter_videos",
                column: "tweet_with_video_id",
                unique: true);
        }
    }
}
