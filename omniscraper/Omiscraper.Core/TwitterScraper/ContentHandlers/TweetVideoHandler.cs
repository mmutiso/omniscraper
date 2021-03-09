using Microsoft.Extensions.Logging;
using Omniscraper.Core.Storage;
using Omniscraper.Core.TwitterScraper;
using Omniscraper.Core.TwitterScraper.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Omniscraper.Core.TwitterScraper.ContentHandlers
{
    public class TweetVideoHandler: AbstractTweetContentHandler
    {
        IScraperRepository scraperRepository;
        ITwitterRepository twitterRepository;
        TweetProcessorSettings settings; 
        public TweetVideoHandler(IScraperRepository repository, 
            ITwitterRepository twitterRepository,  IOptions<TweetProcessorSettings> settings)
        {
            scraperRepository = repository;
            this.twitterRepository = twitterRepository;
            this.settings = settings.Value;
        }

        public override async Task HandleAsync<T>(TwitterStreamModel twitterStreamModel, ILogger<T> logger)
        {
            RawTweetv2 videoTweet = await twitterRepository.FindByIdAsync(twitterStreamModel.IdIfTweetBeingRepliedTo.Value.ToString());


            if (twitterStreamModel.HasVideo())
            {
                TwitterVideo video = twitterStreamModel.GetVideo();

                await scraperRepository.SaveTwitterVideoAsync(video);

                string response = video.GetResponseContent(settings.BaseUrl);
                //send back response
                await twitterRepository.ReplyToTweetAsync(video.RequestingTweetId, response);
                logger.LogInformation($"Sent back this response -> {response}");
            }
            else
            {
                logger.LogWarning($"The tweet being responded to didn't have a video -> {twitterStreamModel.IdIfTweetBeingRepliedTo.Value}");
                await base.HandleAsync(twitterStreamModel, logger);
            }            
        }
    }
}
