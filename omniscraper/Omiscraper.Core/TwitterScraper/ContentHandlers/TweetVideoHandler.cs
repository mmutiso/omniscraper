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
using Omniscraper.Core.Infrastructure;

namespace Omniscraper.Core.TwitterScraper.ContentHandlers
{
    public class TweetVideoHandler: AbstractTweetContentHandler
    {
        IScraperRepository scraperRepository;
        ITwitterRepository twitterRepository;
        TweetProcessorSettings settings;
        VideosApiWrapper videosApiWrapper;
        OpenAICompleter openAICompleter;

        public TweetVideoHandler(IScraperRepository repository,
            VideosApiWrapper videosApiWrapper,
            ITwitterRepository twitterRepository,
            IOptions<TweetProcessorSettings> settings,
            OpenAICompleter openAICompleter)
        {
            scraperRepository = repository;
            this.twitterRepository = twitterRepository;
            this.settings = settings.Value;
            this.videosApiWrapper = videosApiWrapper;
            this.openAICompleter = openAICompleter;
        }

        public override async Task HandleAsync<T>(StreamedTweetContent streamedContent, ILogger<T> logger)
        {
            // replace all this implementation to use the video api. 
            // update video api to provide all other data needed

            VideoResponseModel videoResponseModel = await videosApiWrapper.GetVideoAsync(streamedContent.TweetRepliedToId.ToString());

            if (videoResponseModel.FoundVideo)
            {

                var video = TwitterVideo.Create(videoResponseModel, streamedContent.TweetRepliedToId);
                var request = streamedContent.GenerateVideoRequest(video.Id);

                await scraperRepository.CaptureTwitterVideoAndRequestAsync(request, video);

                var completionResponse = string.Empty;

                try
                {
                    completionResponse = await openAICompleter.GetOpenAICompletionAsync();
                }
                catch(Exception ex)
                {
                    logger.LogError(ex.Message);
                }

                string response = video.GetResponseContent(settings.BaseUrl, request.RequestedBy, completionResponse);

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
