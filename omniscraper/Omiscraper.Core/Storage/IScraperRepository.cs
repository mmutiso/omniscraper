using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.Storage
{
    public interface IScraperRepository
    {
        public Task SaveTwitterVideoAsync(TwitterVideo twitterVideo);
        public Task<TwitterVideo> GetTwitterVideoAsync(Guid id);

        public bool GetIfVideoExists(long tweetId, out TwitterVideo video);
    }
}
