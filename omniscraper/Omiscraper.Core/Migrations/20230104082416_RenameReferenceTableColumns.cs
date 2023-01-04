using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Omniscraper.Core.Migrations
{
    public partial class RenameReferenceTableColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "date_created",
                table: "video_tags",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 4, 8, 24, 16, 465, DateTimeKind.Utc).AddTicks(7794),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 1, 4, 7, 43, 37, 997, DateTimeKind.Utc).AddTicks(1217));

            migrationBuilder.AddColumn<Guid>(
                name: "twittervideo_id",
                table: "twitter_videos_video_tags",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<Guid>(
                name: "videotag_id",
                table: "twitter_videos_video_tags",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "ix_twitter_videos_video_tags_twittervideo_id",
                table: "twitter_videos_video_tags",
                column: "twittervideo_id");

            migrationBuilder.CreateIndex(
                name: "ix_twitter_videos_video_tags_videotag_id",
                table: "twitter_videos_video_tags",
                column: "videotag_id");

            migrationBuilder.AddForeignKey(
                name: "fk_twitter_videos_video_tags_twitter_videos_twitter_video_id",
                table: "twitter_videos_video_tags",
                column: "twittervideo_id",
                principalTable: "twitter_videos",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_twitter_videos_video_tags_video_tag_video_tag_id",
                table: "twitter_videos_video_tags",
                column: "videotag_id",
                principalTable: "video_tags",
                principalColumn: "id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_twitter_videos_video_tags_twitter_videos_twitter_video_id",
                table: "twitter_videos_video_tags");

            migrationBuilder.DropForeignKey(
                name: "fk_twitter_videos_video_tags_video_tag_video_tag_id",
                table: "twitter_videos_video_tags");

            migrationBuilder.DropIndex(
                name: "ix_twitter_videos_video_tags_twittervideo_id",
                table: "twitter_videos_video_tags");

            migrationBuilder.DropIndex(
                name: "ix_twitter_videos_video_tags_videotag_id",
                table: "twitter_videos_video_tags");

            migrationBuilder.DropColumn(
                name: "twittervideo_id",
                table: "twitter_videos_video_tags");

            migrationBuilder.DropColumn(
                name: "videotag_id",
                table: "twitter_videos_video_tags");

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_created",
                table: "video_tags",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 4, 7, 43, 37, 997, DateTimeKind.Utc).AddTicks(1217),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 1, 4, 8, 24, 16, 465, DateTimeKind.Utc).AddTicks(7794));
        }
    }
}
