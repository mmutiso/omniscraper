using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.Storage
{
    public class TwitterThreadRequest
    {
        public Guid Id { get; set; }
        public DateTime DateProcessedUtc { get; set; }
        /// <summary>
        /// This is the @screen_name in Twitter. We will use it when responding to the user.
        /// </summary>
        public string RequestedBy { get; set; }
        public long RequestingTweetId { get; set; }

        public Guid? TwitterThreadId { get; set; }

        public TwitterThread TwitterThread { get; set; }

        public TwitterThreadRequest(long requestingTweetId, Guid threadId, string requestor)
        {
            Id = Guid.NewGuid();
            DateProcessedUtc = DateTime.UtcNow;
            RequestedBy = requestor;
            RequestingTweetId = requestingTweetId;
            TwitterThreadId = threadId;
        }

        public TwitterThreadRequest()
        {

        }
    }
}
