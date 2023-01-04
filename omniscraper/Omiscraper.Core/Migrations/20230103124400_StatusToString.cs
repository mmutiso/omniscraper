using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Omniscraper.Core.Migrations
{
    public partial class StatusToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "date_created",
                table: "video_tags",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 3, 12, 44, 0, 676, DateTimeKind.Utc).AddTicks(9605),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 1, 3, 12, 30, 58, 623, DateTimeKind.Utc).AddTicks(5974));

            migrationBuilder.AlterColumn<int>(
                name: "request_status",
                table: "flag_requests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldDefaultValue: (short)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "date_created",
                table: "video_tags",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(2023, 1, 3, 12, 30, 58, 623, DateTimeKind.Utc).AddTicks(5974),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValue: new DateTime(2023, 1, 3, 12, 44, 0, 676, DateTimeKind.Utc).AddTicks(9605));

            migrationBuilder.AlterColumn<short>(
                name: "request_status",
                table: "flag_requests",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);
        }
    }
}
