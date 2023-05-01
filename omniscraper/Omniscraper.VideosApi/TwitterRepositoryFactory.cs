using LinqToTwitter;
using Omniscraper.Core.Infrastructure;
using Omniscraper.Core.TwitterScraper;

namespace Omniscraper.VideosApi
{
    public class TwitterRepositoryFactory
    {
        OmniScraperContext _context;
        public TwitterRepositoryFactory(OmniScraperContext context)
        {
            _context = context;
        }

        public ITwitterRepository Create()
        {
            ITwitterRepository twitterRepository = new LinqToTwitterRepository(_context);

            return twitterRepository;
        }
    }
}
