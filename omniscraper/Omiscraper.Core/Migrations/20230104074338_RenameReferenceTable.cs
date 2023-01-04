using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Omniscraper.Core.Migrations
{
    public partial class RenameReferenceTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_twitter_video_video_tag_twitter_videos_twitter_videos_id",
                table: "twitter_video_video_tag");

            migrationBuilder.DropForeignKey(
                name: "fk_twitter_video_video_tag_video_tag_video_tags_id",
                table: "twitter_video_video_tag");

            migrationBuilder.DropPrimaryKey(
                name: "pk_twitter_video_video_tag",
                table: "twitter_video_video_tag");

            migrationBuilder.RenameTable(
                name: "twitter_video_video_tag",
                newName: "twitter_videos_video_tags");

            migrationBuilder.RenameIndex(
                name: "ix_twitter_video_video_tag_video_tags_id",
                table: "twitter_videos_video_tags",
                newName: "ix_twitter_videos_video_tags_video_tags_id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_created",
                table: "video_tags",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 4, 7, 43, 37, 997, DateTimeKind.Utc).AddTicks(1217),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 1, 3, 20, 0, 59, 825, DateTimeKind.Utc).AddTicks(3053));

            migrationBuilder.AddPrimaryKey(
                name: "pk_twitter_videos_video_tags",
                table: "twitter_videos_video_tags",
                columns: new[] { "twitter_videos_id", "video_tags_id" });

            migrationBuilder.AddForeignKey(
                name: "fk_twitter_videos_video_tags_twitter_videos_twitter_videos_id",
                table: "twitter_videos_video_tags",
                column: "twitter_videos_id",
                principalTable: "twitter_videos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_twitter_videos_video_tags_video_tag_video_tags_id",
                table: "twitter_videos_video_tags",
                column: "video_tags_id",
                principalTable: "video_tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_twitter_videos_video_tags_twitter_videos_twitter_videos_id",
                table: "twitter_videos_video_tags");

            migrationBuilder.DropForeignKey(
                name: "fk_twitter_videos_video_tags_video_tag_video_tags_id",
                table: "twitter_videos_video_tags");

            migrationBuilder.DropPrimaryKey(
                name: "pk_twitter_videos_video_tags",
                table: "twitter_videos_video_tags");

            migrationBuilder.RenameTable(
                name: "twitter_videos_video_tags",
                newName: "twitter_video_video_tag");

            migrationBuilder.RenameIndex(
                name: "ix_twitter_videos_video_tags_video_tags_id",
                table: "twitter_video_video_tag",
                newName: "ix_twitter_video_video_tag_video_tags_id");

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_created",
                table: "video_tags",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 3, 20, 0, 59, 825, DateTimeKind.Utc).AddTicks(3053),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 1, 4, 7, 43, 37, 997, DateTimeKind.Utc).AddTicks(1217));

            migrationBuilder.AddPrimaryKey(
                name: "pk_twitter_video_video_tag",
                table: "twitter_video_video_tag",
                columns: new[] { "twitter_videos_id", "video_tags_id" });

            migrationBuilder.AddForeignKey(
                name: "fk_twitter_video_video_tag_twitter_videos_twitter_videos_id",
                table: "twitter_video_video_tag",
                column: "twitter_videos_id",
                principalTable: "twitter_videos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_twitter_video_video_tag_video_tag_video_tags_id",
                table: "twitter_video_video_tag",
                column: "video_tags_id",
                principalTable: "video_tags",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
