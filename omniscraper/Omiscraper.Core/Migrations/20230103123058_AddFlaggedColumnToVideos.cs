using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Omniscraper.Core.Migrations
{
    public partial class AddFlaggedColumnToVideos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "date_created",
                table: "video_tags",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 3, 12, 30, 58, 623, DateTimeKind.Utc).AddTicks(5974),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 1, 3, 12, 21, 30, 434, DateTimeKind.Utc).AddTicks(2955));

            migrationBuilder.AddColumn<bool>(
                name: "flagged",
                table: "twitter_videos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "flagged",
                table: "twitter_videos");

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_created",
                table: "video_tags",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 3, 12, 21, 30, 434, DateTimeKind.Utc).AddTicks(2955),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 1, 3, 12, 30, 58, 623, DateTimeKind.Utc).AddTicks(5974));
        }
    }
}
