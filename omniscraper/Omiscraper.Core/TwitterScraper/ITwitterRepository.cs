using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Omniscraper.Core.TwitterScraper;

namespace Omniscraper.Core.TwitterScraper
{
    public interface ITwitterRepository
    {
        public Task<TweetNotification> FindByIdAsync(long id);
    }
}
