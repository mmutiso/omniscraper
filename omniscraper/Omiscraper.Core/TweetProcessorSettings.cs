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
        public string VideoApiBaseUrlKeyVaultName { get; set; }
        public string VideoApiHttpClientName { get; set; }
        public string StreamListeningKeywords = "omniscraper";
        public string OpenaiHttpClientName { get; set; }
    }
}
