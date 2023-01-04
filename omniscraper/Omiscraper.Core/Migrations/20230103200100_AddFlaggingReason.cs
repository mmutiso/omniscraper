using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Omniscraper.Core.Migrations
{
    public partial class AddFlaggingReason : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "date_created",
                table: "video_tags",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 3, 20, 0, 59, 825, DateTimeKind.Utc).AddTicks(3053),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 1, 3, 12, 44, 0, 676, DateTimeKind.Utc).AddTicks(9605));

            migrationBuilder.AddColumn<string>(
                name: "flagging_reason",
                table: "flag_requests",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "flagging_reason",
                table: "flag_requests");

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_created",
                table: "video_tags",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 3, 12, 44, 0, 676, DateTimeKind.Utc).AddTicks(9605),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 1, 3, 20, 0, 59, 825, DateTimeKind.Utc).AddTicks(3053));
        }
    }
}
