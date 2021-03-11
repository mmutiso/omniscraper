using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Omniscraper.Core.Migrations
{
    public partial class SplitVideoEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "requested_by",
                table: "twitter_videos");

            migrationBuilder.DropColumn(
                name: "requesting_tweet_id",
                table: "twitter_videos");

            migrationBuilder.RenameColumn(
                name: "tweet_with_video_id",
                table: "twitter_videos",
                newName: "parent_tweet_id");

            migrationBuilder.RenameColumn(
                name: "date_processed_utc",
                table: "twitter_videos",
                newName: "date_saved_utc");

            migrationBuilder.RenameIndex(
                name: "ix_twitter_videos_tweet_with_video_id",
                table: "twitter_videos",
                newName: "ix_twitter_videos_parent_tweet_id");

            migrationBuilder.CreateTable(
                name: "twitter_video_request",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    date_processed_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    requested_by = table.Column<string>(type: "text", nullable: true),
                    requesting_tweet_id = table.Column<long>(type: "bigint", nullable: false),
                    twitter_video_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_twitter_video_request", x => x.id);
                    table.ForeignKey(
                        name: "fk_twitter_video_request_twitter_videos_twitter_video_id",
                        column: x => x.twitter_video_id,
                        principalTable: "twitter_videos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_twitter_video_request_twitter_video_id",
                table: "twitter_video_request",
                column: "twitter_video_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "twitter_video_request");

            migrationBuilder.RenameColumn(
                name: "parent_tweet_id",
                table: "twitter_videos",
                newName: "tweet_with_video_id");

            migrationBuilder.RenameColumn(
                name: "date_saved_utc",
                table: "twitter_videos",
                newName: "date_processed_utc");

            migrationBuilder.RenameIndex(
                name: "ix_twitter_videos_parent_tweet_id",
                table: "twitter_videos",
                newName: "ix_twitter_videos_tweet_with_video_id");

            migrationBuilder.AddColumn<string>(
                name: "requested_by",
                table: "twitter_videos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "requesting_tweet_id",
                table: "twitter_videos",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
