using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Omniscraper.Core.Migrations
{
    public partial class snake_case_named_columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "twitter_videos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    url = table.Column<string>(type: "text", nullable: true),
                    date_processed_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    tweet_id = table.Column<long>(type: "bigint", nullable: false),
                    parent_tweet_id = table.Column<long>(type: "bigint", nullable: false),
                    requested_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_twitter_videos", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "twitter_videos");
        }
    }
}
