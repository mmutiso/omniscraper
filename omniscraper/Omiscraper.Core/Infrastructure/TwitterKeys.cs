using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.Infrastructure
{
    public record TwitterKeys
    {
        public string ConsumerKey { get; init; }
        public string ConsumerSecret { get; init; }
        public string AccessToken { get; init; }
        public string AccessTokenSecret { get; init; }
    }
}
