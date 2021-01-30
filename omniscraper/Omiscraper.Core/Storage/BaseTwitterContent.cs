﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.Storage
{
    public class BaseTwitterContent
    {
        public Guid Id { get; set; }
        public DateTime DateProcessedUTC { get; set; }

        public long TweetWithVideoId { get; set; }
        public string Slug { get; set; }
        public long RequestingTweetId { get; set; }
        /// <summary>
        /// This is the @screen_name in Twitter. We will use it when responding to the user.
        /// </summary>
        public string RequestedBy { get; set; }

        public BaseTwitterContent()
        {

        }

        public BaseTwitterContent(Guid id, DateTime dateProcessedUTC, long tweetId, long parentTweetId)
        {
            Id = id;
            DateProcessedUTC = dateProcessedUTC;
            TweetWithVideoId = tweetId;
            RequestingTweetId = parentTweetId;
            Slug = id.ToString().Split("-")[0];
        }
    }
}
