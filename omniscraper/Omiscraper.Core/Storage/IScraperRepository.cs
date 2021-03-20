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
        public Task SaveAsync<T>(T entity);
        public Task SaveAsync<T, U>(T entity, U anotherEntity);
        public Task<TwitterVideo> GetTwitterVideoAsync(Guid id);

        public bool GetIfVideoExists(long tweetId, out TwitterVideo video);
        public bool GetThreadIfExists(long conversationId, out TwitterThread thread);
    }
}
