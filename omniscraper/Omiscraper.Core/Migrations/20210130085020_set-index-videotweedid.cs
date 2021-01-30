using Microsoft.EntityFrameworkCore.Migrations;

namespace Omniscraper.Core.Migrations
{
    public partial class setindexvideotweedid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_twitter_videos_tweet_with_video_id",
                table: "twitter_videos",
                column: "tweet_with_video_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_twitter_videos_tweet_with_video_id",
                table: "twitter_videos");
        }
    }
}
