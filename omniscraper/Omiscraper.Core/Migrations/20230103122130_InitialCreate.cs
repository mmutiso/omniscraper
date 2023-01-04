using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Omniscraper.Core.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "flag_requests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    slug = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    twitter_handle = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_requested = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    date_of_action = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    request_status = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_flag_requests", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "twitter_videos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    date_saved_utc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    slug = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
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
                name: "video_tags",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    tag_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    slug = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    date_created = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValue: new DateTime(2023, 1, 3, 12, 21, 30, 434, DateTimeKind.Utc).AddTicks(2955))
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_video_tags", x => x.id);
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

            migrationBuilder.CreateTable(
                name: "twitter_video_video_tag",
                columns: table => new
                {
                    twitter_videos_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    video_tags_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_twitter_video_video_tag", x => new { x.twitter_videos_id, x.video_tags_id });
                    table.ForeignKey(
                        name: "fk_twitter_video_video_tag_twitter_videos_twitter_videos_id",
                        column: x => x.twitter_videos_id,
                        principalTable: "twitter_videos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_twitter_video_video_tag_video_tag_video_tags_id",
                        column: x => x.video_tags_id,
                        principalTable: "video_tags",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "ix_twitter_video_requests_twitter_video_id",
                table: "twitter_video_requests",
                column: "twitter_video_id");

            migrationBuilder.CreateIndex(
                name: "ix_twitter_video_video_tag_video_tags_id",
                table: "twitter_video_video_tag",
                column: "video_tags_id");

            migrationBuilder.CreateIndex(
                name: "ix_twitter_videos_parent_tweet_id",
                table: "twitter_videos",
                column: "parent_tweet_id");

            migrationBuilder.CreateIndex(
                name: "ix_twitter_videos_slug",
                table: "twitter_videos",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_video_tags_slug",
                table: "video_tags",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_video_tags_tag_name",
                table: "video_tags",
                column: "tag_name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "flag_requests");

            migrationBuilder.DropTable(
                name: "twitter_video_requests");

            migrationBuilder.DropTable(
                name: "twitter_video_video_tag");

            migrationBuilder.DropTable(
                name: "twitter_videos");

            migrationBuilder.DropTable(
                name: "video_tags");
        }
    }
}
