using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.Storage
{
    public class TwitterVideoRequest
    {
        public Guid Id { get; set; }
        public DateTime DateProcessedUtc { get; set; }
        public bool Fulfilled { get; set; }
        /// <summary>
        /// This is the @screen_name in Twitter. We will use it when responding to the user.
        /// </summary>
        public string RequestedBy { get; set; }
        public long RequestingTweetId { get; set; }

        public Guid? TwitterVideoId { get; set; }

        public TwitterVideo TwitterVideo { get; set; }

        public TwitterVideoRequest(bool fulfilled, long requestingTweetId, Guid videoId, string requestor)
        {
            Id = Guid.NewGuid();
            DateProcessedUtc = DateTime.UtcNow;
            Fulfilled = fulfilled;
            RequestedBy = requestor;
            RequestingTweetId = requestingTweetId;
            TwitterVideoId = videoId;
        }

        public TwitterVideoRequest()
        {

        }

    }
}
