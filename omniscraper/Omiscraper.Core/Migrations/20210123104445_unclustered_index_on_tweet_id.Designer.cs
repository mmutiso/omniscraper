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
    [Migration("20210123104445_unclustered_index_on_tweet_id")]
    partial class unclustered_index_on_tweet_id
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

                    b.Property<DateTime>("DateProcessedUTC")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("date_processed_utc");

                    b.Property<long>("ParentTweetId")
                        .HasColumnType("bigint")
                        .HasColumnName("parent_tweet_id");

                    b.Property<string>("RequestedBy")
                        .HasColumnType("text")
                        .HasColumnName("requested_by");

                    b.Property<long>("TweetId")
                        .HasColumnType("bigint")
                        .HasColumnName("tweet_id");

                    b.Property<string>("Url")
                        .HasColumnType("text")
                        .HasColumnName("url");

                    b.HasKey("Id")
                        .HasName("pk_twitter_videos");

                    b.HasIndex("TweetId")
                        .IsUnique()
                        .HasDatabaseName("ix_twitter_videos_tweet_id");

                    b.ToTable("twitter_videos");
                });
#pragma warning restore 612, 618
        }
    }
}
