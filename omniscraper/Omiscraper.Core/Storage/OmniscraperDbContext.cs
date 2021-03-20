using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.Storage
{
    public class OmniscraperDbContext : DbContext
    {

        public OmniscraperDbContext(DbContextOptions<OmniscraperDbContext> dbContextOptions)
            :base(dbContextOptions)
        {

        }
        public DbSet<TwitterVideo> TwitterVideos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TwitterVideo>(options =>
            {
                options.ToTable("twitter_videos");
                options.HasIndex(x => x.Slug).IsUnique();
                options.HasIndex(x => x.ParentTweetId);
            });

            builder.Entity<TwitterVideoRequest>(options =>
            {
                options.ToTable("twitter_video_requests");
                options.HasOne(x => x.TwitterVideo)
                .WithMany(x => x.TwitterVideoRequests);

            });

            builder.Entity<TwitterThreadRequest>(options =>
            {
                options.ToTable("twitter_thread_requests");
                options.HasOne(x => x.TwitterThread)
                .WithMany(x => x.ThreadRequests);
            });

            builder.Entity<TwitterThread>(options =>
            {
                options.ToTable("twitter_threads");
                options.HasIndex(x => x.Slug).IsUnique();
                options.HasIndex(x => x.ConversationId);
                options.Property(x => x.Tweets).HasColumnType("jsonb");
            });
        }
    }
}
