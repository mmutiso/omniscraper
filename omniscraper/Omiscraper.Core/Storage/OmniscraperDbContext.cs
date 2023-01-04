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
                options.Property(x => x.Slug).HasMaxLength(10);
                options.HasIndex(x => x.Slug).IsUnique();
                options.HasIndex(x => x.ParentTweetId);
                options.Property(x => x.Text).HasMaxLength(290);
                options.Property(x=>x.Flagged).HasDefaultValue(false);

                options.HasMany(x => x.VideoTags)
                       .WithMany(y => y.TwitterVideos)
                       .UsingEntity<Dictionary<string, object>>(rt=>
                       {
                           rt.ToTable("twitter_videos_video_tags");
                           rt.HasOne<TwitterVideo>().WithMany().HasForeignKey("twittervideo_id");
                           rt.HasOne<VideoTag>().WithMany().HasForeignKey("videotag_id");
                       });
            });

            builder.Entity<TwitterVideoRequest>(options =>
            {
                options.ToTable("twitter_video_requests");
                options.HasOne(x => x.TwitterVideo)
                .WithMany(x => x.TwitterVideoRequests);
            });

            builder.Entity<VideoTag>(options =>
            {
                options.ToTable("video_tags");
                options.Property(x => x.TagName).HasMaxLength(100);
                options.HasIndex(x => x.TagName).IsUnique();

                options.Property(x => x.Slug).HasMaxLength(100);
                options.HasIndex(x => x.Slug).IsUnique();

                options.Property(x => x.DateCreated).HasDefaultValue(DateTime.UtcNow);
            });

            builder.Entity<FlagRequest>(options =>
            {
                options.Property(x => x.RequestStatus).HasDefaultValue(RequestStatus.Pending);
                options.ToTable("flag_requests");
            });
        }
    }
}
