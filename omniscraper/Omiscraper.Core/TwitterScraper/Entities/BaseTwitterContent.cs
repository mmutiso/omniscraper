using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.TwitterScraper.Entities
{
    public class BaseTwitterContent
    {
        public Guid Id { get; set; }
        public DateTime DateProcessed { get; set; }
        public string ParentTweetId { get; set; }
        /// <summary>
        /// This is the @screen_name in Twitter. We will use it when responding to the user.
        /// </summary>
        public string RequestedBy { get; set; }
    }
}
