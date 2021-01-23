using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Omniscraper.Core.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TwitterVideos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: true),
                    DateProcessedUTC = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TweetId = table.Column<long>(type: "INTEGER", nullable: false),
                    ParentTweetId = table.Column<long>(type: "INTEGER", nullable: false),
                    RequestedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TwitterVideos", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TwitterVideos");
        }
    }
}
