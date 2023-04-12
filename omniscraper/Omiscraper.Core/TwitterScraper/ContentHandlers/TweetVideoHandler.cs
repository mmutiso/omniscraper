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
using Omniscraper.Core.TwitterScraper.Entities.v2;

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

        public override async Task HandleAsync<T>(StreamedTweetContent streamedContent, ILogger<T> logger)
        {
            // replace all this implementation to use the video api. 
            // update video api to provide all other data needed

            VideoResponseModel videoResponseModel = default;

            if (videoResponseModel.FoundVideo)
            {
               
                var request = streamedContent.GenerateVideoRequest();
                var video = TwitterVideo.Create(request.Id, videoResponseModel, streamedContent.TweetRepliedToId.Value);

                await scraperRepository.CaptureTwitterVideoAndRequestAsync(request, video);

                string response = video.GetResponseContent(settings.BaseUrl, request.RequestedBy);
                //send back response
                await twitterRepository.ReplyToTweetAsync(request.RequestingTweetId.ToString(), response);
                logger.LogInformation($"Sent back this response -> {response}");
            }
            else
            {
                logger.LogWarning($"The tweet being responded to didn't have a video -> {streamedContent.TweetRepliedToId}");
                await base.HandleAsync(streamedContent, logger);
            }            
        }
    }
}
