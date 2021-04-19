using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Omniscraper.Core.Storage;

namespace Omniscraper.Core.Migrations
{
    public partial class addthreadstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "twitter_threads",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    slug = table.Column<string>(type: "text", nullable: true),
                    conversation_id = table.Column<long>(type: "bigint", nullable: false),
                    author_display_name = table.Column<string>(type: "text", nullable: true),
                    author_twitter_username = table.Column<string>(type: "text", nullable: true),
                    author_profile_picture_link = table.Column<string>(type: "text", nullable: true),
                    tweets = table.Column<List<TweetContent>>(type: "jsonb", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_twitter_threads", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "twitter_thread_requests",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    date_processed_utc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    requested_by = table.Column<string>(type: "text", nullable: true),
                    requesting_tweet_id = table.Column<long>(type: "bigint", nullable: false),
                    twitter_thread_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_twitter_thread_requests", x => x.id);
                    table.ForeignKey(
                        name: "fk_twitter_thread_requests_twitter_thread_twitter_thread_id",
                        column: x => x.twitter_thread_id,
                        principalTable: "twitter_threads",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_twitter_thread_requests_twitter_thread_id",
                table: "twitter_thread_requests",
                column: "twitter_thread_id");

            migrationBuilder.CreateIndex(
                name: "ix_twitter_threads_conversation_id",
                table: "twitter_threads",
                column: "conversation_id");

            migrationBuilder.CreateIndex(
                name: "ix_twitter_threads_slug",
                table: "twitter_threads",
                column: "slug",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "twitter_thread_requests");

            migrationBuilder.DropTable(
                name: "twitter_threads");
        }
    }
}
