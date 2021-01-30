using Microsoft.EntityFrameworkCore.Migrations;

namespace Omniscraper.Core.Migrations
{
    public partial class RenameColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "tweet_id",
                table: "twitter_videos",
                newName: "tweet_with_video_id");

            migrationBuilder.RenameColumn(
                name: "parent_tweet_id",
                table: "twitter_videos",
                newName: "requesting_tweet_id");

            migrationBuilder.RenameIndex(
                name: "ix_twitter_videos_tweet_id",
                table: "twitter_videos",
                newName: "ix_twitter_videos_tweet_with_video_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "tweet_with_video_id",
                table: "twitter_videos",
                newName: "tweet_id");

            migrationBuilder.RenameColumn(
                name: "requesting_tweet_id",
                table: "twitter_videos",
                newName: "parent_tweet_id");

            migrationBuilder.RenameIndex(
                name: "ix_twitter_videos_tweet_with_video_id",
                table: "twitter_videos",
                newName: "ix_twitter_videos_tweet_id");
        }
    }
}
