﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Omniscraper.Core.Storage;

namespace Omniscraper.Core.Migrations
{
    [DbContext(typeof(OmniscraperDbContext))]
    [Migration("20210310120124_SplitVideoEntity")]
    partial class SplitVideoEntity
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("Omniscraper.Core.Storage.TwitterVideo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("DateSavedUTC")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("date_saved_utc");

                    b.Property<long>("ParentTweetId")
                        .HasColumnType("bigint")
                        .HasColumnName("parent_tweet_id");

                    b.Property<string>("Slug")
                        .HasColumnType("text")
                        .HasColumnName("slug");

                    b.Property<string>("Url")
                        .HasColumnType("text")
                        .HasColumnName("url");

                    b.HasKey("Id")
                        .HasName("pk_twitter_videos");

                    b.HasIndex("ParentTweetId")
                        .HasDatabaseName("ix_twitter_videos_parent_tweet_id");

                    b.HasIndex("Slug")
                        .IsUnique()
                        .HasDatabaseName("ix_twitter_videos_slug");

                    b.ToTable("twitter_videos");
                });

            modelBuilder.Entity("Omniscraper.Core.Storage.TwitterVideoRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<DateTime>("DateProcessedUtc")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("date_processed_utc");

                    b.Property<string>("RequestedBy")
                        .HasColumnType("text")
                        .HasColumnName("requested_by");

                    b.Property<long>("RequestingTweetId")
                        .HasColumnType("bigint")
                        .HasColumnName("requesting_tweet_id");

                    b.Property<Guid?>("TwitterVideoId")
                        .HasColumnType("uuid")
                        .HasColumnName("twitter_video_id");

                    b.HasKey("Id")
                        .HasName("pk_twitter_video_request");

                    b.HasIndex("TwitterVideoId")
                        .HasDatabaseName("ix_twitter_video_request_twitter_video_id");

                    b.ToTable("twitter_video_request");
                });

            modelBuilder.Entity("Omniscraper.Core.Storage.TwitterVideoRequest", b =>
                {
                    b.HasOne("Omniscraper.Core.Storage.TwitterVideo", "TwitterVideo")
                        .WithMany("TwitterVideoRequests")
                        .HasForeignKey("TwitterVideoId")
                        .HasConstraintName("fk_twitter_video_request_twitter_videos_twitter_video_id");

                    b.Navigation("TwitterVideo");
                });

            modelBuilder.Entity("Omniscraper.Core.Storage.TwitterVideo", b =>
                {
                    b.Navigation("TwitterVideoRequests");
                });
#pragma warning restore 612, 618
        }
    }
}
