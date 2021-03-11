using Microsoft.EntityFrameworkCore.Migrations;

namespace Omniscraper.Core.Migrations
{
    public partial class CreateRelationships : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_twitter_video_request_twitter_videos_twitter_video_id",
                table: "twitter_video_request");

            migrationBuilder.DropPrimaryKey(
                name: "pk_twitter_video_request",
                table: "twitter_video_request");

            migrationBuilder.RenameTable(
                name: "twitter_video_request",
                newName: "twitter_video_requests");

            migrationBuilder.RenameIndex(
                name: "ix_twitter_video_request_twitter_video_id",
                table: "twitter_video_requests",
                newName: "ix_twitter_video_requests_twitter_video_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_twitter_video_requests",
                table: "twitter_video_requests",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_twitter_video_requests_twitter_videos_twitter_video_id",
                table: "twitter_video_requests",
                column: "twitter_video_id",
                principalTable: "twitter_videos",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_twitter_video_requests_twitter_videos_twitter_video_id",
                table: "twitter_video_requests");

            migrationBuilder.DropPrimaryKey(
                name: "pk_twitter_video_requests",
                table: "twitter_video_requests");

            migrationBuilder.RenameTable(
                name: "twitter_video_requests",
                newName: "twitter_video_request");

            migrationBuilder.RenameIndex(
                name: "ix_twitter_video_requests_twitter_video_id",
                table: "twitter_video_request",
                newName: "ix_twitter_video_request_twitter_video_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_twitter_video_request",
                table: "twitter_video_request",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_twitter_video_request_twitter_videos_twitter_video_id",
                table: "twitter_video_request",
                column: "twitter_video_id",
                principalTable: "twitter_videos",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
