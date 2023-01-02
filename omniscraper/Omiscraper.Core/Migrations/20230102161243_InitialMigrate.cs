using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Omniscraper.Core.Migrations
{
    public partial class InitialMigrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "twitter_videos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    date_saved_utc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    slug = table.Column<string>(type: "varchar(95)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    url = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    parent_tweet_id = table.Column<long>(type: "bigint", nullable: false),
                    video_thumbnail_link_https = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    text = table.Column<string>(type: "varchar(290)", maxLength: 290, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_twitter_videos", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "twitter_video_requests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    date_processed_utc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    requested_by = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    requesting_tweet_id = table.Column<long>(type: "bigint", nullable: false),
                    twitter_video_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_twitter_video_requests", x => x.id);
                    table.ForeignKey(
                        name: "fk_twitter_video_requests_twitter_videos_twitter_video_id",
                        column: x => x.twitter_video_id,
                        principalTable: "twitter_videos",
                        principalColumn: "id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "ix_twitter_video_requests_twitter_video_id",
                table: "twitter_video_requests",
                column: "twitter_video_id");

            migrationBuilder.CreateIndex(
                name: "ix_twitter_videos_parent_tweet_id",
                table: "twitter_videos",
                column: "parent_tweet_id");

            migrationBuilder.CreateIndex(
                name: "ix_twitter_videos_slug",
                table: "twitter_videos",
                column: "slug",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "twitter_video_requests");

            migrationBuilder.DropTable(
                name: "twitter_videos");
        }
    }
}
