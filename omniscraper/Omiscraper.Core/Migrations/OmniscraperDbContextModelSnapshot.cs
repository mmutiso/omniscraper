﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Omniscraper.Core.Storage;

namespace Omniscraper.Core.Migrations
{
    [DbContext(typeof(OmniscraperDbContext))]
    partial class OmniscraperDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<DateTime>("DateProcessedUTC")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("date_processed_utc");

                    b.Property<string>("RequestedBy")
                        .HasColumnType("text")
                        .HasColumnName("requested_by");

                    b.Property<long>("RequestingTweetId")
                        .HasColumnType("bigint")
                        .HasColumnName("requesting_tweet_id");

                    b.Property<string>("Slug")
                        .HasColumnType("text")
                        .HasColumnName("slug");

                    b.Property<long>("TweetWithVideoId")
                        .HasColumnType("bigint")
                        .HasColumnName("tweet_with_video_id");

                    b.Property<string>("Url")
                        .HasColumnType("text")
                        .HasColumnName("url");

                    b.HasKey("Id")
                        .HasName("pk_twitter_videos");

                    b.HasIndex("Slug")
                        .IsUnique()
                        .HasDatabaseName("ix_twitter_videos_slug");

                    b.HasIndex("TweetWithVideoId")
                        .HasDatabaseName("ix_twitter_videos_tweet_with_video_id");

                    b.ToTable("twitter_videos");
                });
#pragma warning restore 612, 618
        }
    }
}
