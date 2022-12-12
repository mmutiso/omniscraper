using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Omniscraper.Core.TwitterScraper.Entities
{
    public class TweetEntities
    {
        public Hashtag[] hashtags { get; set; }
        public TwitterUrls[] urls { get; set; }
        public UserMention[] user_mentions { get; set; }
        public string[] symbols { get; set; }
        public TweetMedia[] MediaEntities { get; set; }
    }

    public class Hashtag
    {
        public string text { get; set; }
        public int[] indices { get; set; }
    }

    public class UserMention
    {
        public string screen_name { get; set; }
        public string name { get; set; }
        public long? id { get; set; }
        public string id_str { get; set; }
        public int[] indices { get; set; }
    }

    public class TwitterUrls
    {
        public string url { get; set; }
        public string expanded_url { get; set; }
        public string display_url { get; set; }
        public int?[] indices { get; set; }
    }

    public class TweetMedia
    {
        public long id { get; set; }
        public string id_str { get; set; }
        public int[] indices { get; set; }
        public string MediaUrl { get; set; }
        public string MediaUrlHttps { get; set; }
        public string url { get; set; }
        public string display_url { get; set; }
        public string expanded_url { get; set; }
        public string type { get; set; }

        [JsonIgnore]
        public Sizes[] sizes { get; set; }
    }

    public class Large
    {
        public int w { get; set; }
        public int h { get; set; }
        public string resize { get; set; }
    }

    public class Small
    {
        public int w { get; set; }
        public int h { get; set; }
        public string resize { get; set; }
    }

    public class Medium
    {
        public int w { get; set; }
        public int h { get; set; }
        public string resize { get; set; }
    }

    public class Thumb
    {
        public int w { get; set; }
        public int h { get; set; }
        public string resize { get; set; }
    }

    public class Sizes
    {
        public Large large { get; set; }
        public Small small { get; set; }
        public Medium medium { get; set; }
        public Thumb thumb { get; set; }
    }
}
