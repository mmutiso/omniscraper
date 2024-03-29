﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Omniscraper.Core.Storage;

#nullable disable

namespace Omniscraper.Core.Migrations
{
    [DbContext(typeof(OmniscraperDbContext))]
    [Migration("20230103122130_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Omniscraper.Core.Storage.FlagRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("id");

                    b.Property<DateTime>("DateOfAction")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("date_of_action");

                    b.Property<DateTime>("DateRequested")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("date_requested");

                    b.Property<short>("RequestStatus")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasDefaultValue((short)0)
                        .HasColumnName("request_status");

                    b.Property<string>("Slug")
                        .HasColumnType("longtext")
                        .HasColumnName("slug");

                    b.Property<string>("TwitterHandle")
                        .HasColumnType("longtext")
                        .HasColumnName("twitter_handle");

                    b.HasKey("Id")
                        .HasName("pk_flag_requests");

                    b.ToTable("flag_requests", (string)null);
                });

            modelBuilder.Entity("Omniscraper.Core.Storage.TwitterVideo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("id");

                    b.Property<DateTime>("DateSavedUTC")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("date_saved_utc");

                    b.Property<long>("ParentTweetId")
                        .HasColumnType("bigint")
                        .HasColumnName("parent_tweet_id");

                    b.Property<string>("Slug")
                        .HasMaxLength(10)
                        .HasColumnType("varchar(10)")
                        .HasColumnName("slug");

                    b.Property<string>("Text")
                        .HasMaxLength(290)
                        .HasColumnType("varchar(290)")
                        .HasColumnName("text");

                    b.Property<string>("Url")
                        .HasColumnType("longtext")
                        .HasColumnName("url");

                    b.Property<string>("VideoThumbnailLinkHttps")
                        .HasColumnType("longtext")
                        .HasColumnName("video_thumbnail_link_https");

                    b.HasKey("Id")
                        .HasName("pk_twitter_videos");

                    b.HasIndex("ParentTweetId")
                        .HasDatabaseName("ix_twitter_videos_parent_tweet_id");

                    b.HasIndex("Slug")
                        .IsUnique()
                        .HasDatabaseName("ix_twitter_videos_slug");

                    b.ToTable("twitter_videos", (string)null);
                });

            modelBuilder.Entity("Omniscraper.Core.Storage.TwitterVideoRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("id");

                    b.Property<DateTime>("DateProcessedUtc")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("date_processed_utc");

                    b.Property<string>("RequestedBy")
                        .HasColumnType("longtext")
                        .HasColumnName("requested_by");

                    b.Property<long>("RequestingTweetId")
                        .HasColumnType("bigint")
                        .HasColumnName("requesting_tweet_id");

                    b.Property<Guid?>("TwitterVideoId")
                        .HasColumnType("char(36)")
                        .HasColumnName("twitter_video_id");

                    b.HasKey("Id")
                        .HasName("pk_twitter_video_requests");

                    b.HasIndex("TwitterVideoId")
                        .HasDatabaseName("ix_twitter_video_requests_twitter_video_id");

                    b.ToTable("twitter_video_requests", (string)null);
                });

            modelBuilder.Entity("Omniscraper.Core.Storage.VideoTag", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)")
                        .HasColumnName("id");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)")
                        .HasDefaultValue(new DateTime(2023, 1, 3, 12, 21, 30, 434, DateTimeKind.Utc).AddTicks(2955))
                        .HasColumnName("date_created");

                    b.Property<string>("Description")
                        .HasColumnType("longtext")
                        .HasColumnName("description");

                    b.Property<string>("Slug")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("slug");

                    b.Property<string>("TagName")
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)")
                        .HasColumnName("tag_name");

                    b.HasKey("Id")
                        .HasName("pk_video_tags");

                    b.HasIndex("Slug")
                        .IsUnique()
                        .HasDatabaseName("ix_video_tags_slug");

                    b.HasIndex("TagName")
                        .IsUnique()
                        .HasDatabaseName("ix_video_tags_tag_name");

                    b.ToTable("video_tags", (string)null);
                });

            modelBuilder.Entity("TwitterVideoVideoTag", b =>
                {
                    b.Property<Guid>("TwitterVideosId")
                        .HasColumnType("char(36)")
                        .HasColumnName("twitter_videos_id");

                    b.Property<Guid>("VideoTagsId")
                        .HasColumnType("char(36)")
                        .HasColumnName("video_tags_id");

                    b.HasKey("TwitterVideosId", "VideoTagsId")
                        .HasName("pk_twitter_video_video_tag");

                    b.HasIndex("VideoTagsId")
                        .HasDatabaseName("ix_twitter_video_video_tag_video_tags_id");

                    b.ToTable("twitter_video_video_tag", (string)null);
                });

            modelBuilder.Entity("Omniscraper.Core.Storage.TwitterVideoRequest", b =>
                {
                    b.HasOne("Omniscraper.Core.Storage.TwitterVideo", "TwitterVideo")
                        .WithMany("TwitterVideoRequests")
                        .HasForeignKey("TwitterVideoId")
                        .HasConstraintName("fk_twitter_video_requests_twitter_videos_twitter_video_id");

                    b.Navigation("TwitterVideo");
                });

            modelBuilder.Entity("TwitterVideoVideoTag", b =>
                {
                    b.HasOne("Omniscraper.Core.Storage.TwitterVideo", null)
                        .WithMany()
                        .HasForeignKey("TwitterVideosId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_twitter_video_video_tag_twitter_videos_twitter_videos_id");

                    b.HasOne("Omniscraper.Core.Storage.VideoTag", null)
                        .WithMany()
                        .HasForeignKey("VideoTagsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_twitter_video_video_tag_video_tag_video_tags_id");
                });

            modelBuilder.Entity("Omniscraper.Core.Storage.TwitterVideo", b =>
                {
                    b.Navigation("TwitterVideoRequests");
                });
#pragma warning restore 612, 618
        }
    }
}
