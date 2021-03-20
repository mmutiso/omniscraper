using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.Storage
{
    public interface IScraperRepository
    {
        public Task CaptureTwitterVideoAndRequestAsync(TwitterVideoRequest request, TwitterVideo twitterVideo);
        public Task CaptureTwitterRequestAsync(TwitterVideoRequest request);
        public Task<TwitterVideo> GetTwitterVideoAsync(Guid id);

        public bool GetIfVideoExists(long tweetId, out TwitterVideo video);
        public Task SaveThreadAsync(TwitterThread twitterThread);
    }
}
