using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Omniscraper.Api.Models
{
    public record TwitterKeys
    {
        public string ConsumerKey { get; init; }
        public string ConsumerSecret { get; init; }
        public string AccessToken { get; init; }
        public string AccessTokenSecret { get; init; }
    }
}
