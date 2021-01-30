using Omniscraper.Core.Storage;
using Omniscraper.Core.TwitterScraper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Omniscraper.Core.TwitterScraper.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Omniscraper.Core
{
    public class TweetProcessingService
    {
        ITwitterRepository twitterRepository;
        IScraperRepository scraperRepository;
        ILogger<TweetProcessingService> logger;
        TweetProcessorSettings settings; 

        public TweetProcessingService(ITwitterRepository twitterRepository, IScraperRepository scraperRepository,
            ILogger<TweetProcessingService> logger, IOptions<TweetProcessorSettings> options)
        {
            this.twitterRepository = twitterRepository;
            this.scraperRepository = scraperRepository;
            this.logger = logger;
            settings = options.Value;
        }

        public async Task ProcessTweetAsync(string tweetJsonString)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //deserialize tweet.
            RawTweet requestingTweet = DeserializeTweet(tweetJsonString);
            //determine intent.
            if (requestingTweet.user.screen_name.Equals("omniscraper", StringComparison.OrdinalIgnoreCase))
            {
                logger.LogInformation("This is my tweet. Ignoring");
                return;
            }

            if (!requestingTweet.in_reply_to_status_id.HasValue) // this tweet is not in response to any tweet
            {
                if (logger.IsEnabled(LogLevel.Warning))
                {
                    logger.LogWarning("Received tweet with id {} that is not a reply to another", requestingTweet.id);
                }
                return;
            }

            TwitterVideo twitterVideo;
            bool exists = scraperRepository.GetIfVideoExists(requestingTweet.in_reply_to_status_id.Value, out twitterVideo);
            if (exists)
            {
                logger.LogInformation($"This tweet existed {requestingTweet.in_reply_to_status_id.Value}.");

                var mutatedVideo = twitterVideo.CreateForNewTweet(requestingTweet.id, requestingTweet.user.screen_name);
                await scraperRepository.SaveTwitterVideoAsync(mutatedVideo);

                string response = mutatedVideo.GetResponseContent(settings.BaseUrl);
                //send back response
                await twitterRepository.ReplyToTweetAsync(mutatedVideo.RequestingTweetId, response);
                stopwatch.Stop();
                logger.LogInformation($"Processed video request in {stopwatch.ElapsedMilliseconds}ms");
                return;
            }

            RawTweet videoTweet = await twitterRepository.FindByIdAsync(requestingTweet.in_reply_to_status_id.Value);
            TweetNotification tweet = new TweetNotification(videoTweet, requestingTweet.id, requestingTweet.user.screen_name);

            //handle intent
            if (tweet.HasVideo())
            {
                var video = tweet.GetVideo();

                await scraperRepository.SaveTwitterVideoAsync(video);

                string response = video.GetResponseContent(settings.BaseUrl);
                //send back response
                await twitterRepository.ReplyToTweetAsync(video.RequestingTweetId, response);
                stopwatch.Stop();
                logger.LogInformation($"Processed video request in {stopwatch.ElapsedMilliseconds}ms");
                return;
            }

            logger.LogWarning($"The tweet being responded to didn't have a video -> {videoTweet.id}");
            return;

        }

       

        private RawTweet DeserializeTweet(string tweetJsonString)
        {
            var tweet = JsonConvert.DeserializeObject<RawTweet>(tweetJsonString);

            return tweet;
        }
    }
}
