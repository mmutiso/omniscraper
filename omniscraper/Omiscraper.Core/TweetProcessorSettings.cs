using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core
{
    public class TweetProcessorSettings
    {
        public string BaseUrl { get; set; }
        /// <summary>
        /// Comma separated list of keywords to listen to
        /// </summary>
        public string StreamListeningKeywords { get; set; }
    }
}
