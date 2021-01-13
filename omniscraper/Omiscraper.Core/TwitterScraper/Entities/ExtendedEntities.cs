using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.TwitterScraper.Entities
{
    public class Size
    {
        public string Type { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Resize { get; set; }
    }

    public class AspectRatio
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class Variant
    {
        public int BitRate { get; set; }
        public string ContentType { get; set; }
        public string Url { get; set; }
    }

    public class VideoInfo
    {
        public AspectRatio AspectRatio { get; set; }
        public int Duration { get; set; }
        public List<Variant> Variants { get; set; }
    }

    public class MediaEntity
    {
        public long ID { get; set; }
        public string MediaUrl { get; set; }
        public object AltText { get; set; }
        public string MediaUrlHttps { get; set; }
        public List<Size> Sizes { get; set; }
        public string Type { get; set; }
        public List<int> Indices { get; set; }
        public object indices { get; set; }
        public VideoInfo VideoInfo { get; set; }
        public string url { get; set; }
        public string display_url { get; set; }
        public string expanded_url { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
    }

    public class ExtendedEntities
    {
        public List<object> user_mentions { get; set; }
        public List<object> urls { get; set; }
        public List<object> hashtags { get; set; }
        public List<MediaEntity> MediaEntities { get; set; }
        public List<object> symbols { get; set; }
    }
}
