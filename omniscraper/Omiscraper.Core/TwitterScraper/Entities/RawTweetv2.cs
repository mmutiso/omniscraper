using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Omniscraper.Core.TwitterScraper.Entities
{
    public class RawTweetv2
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }

        [JsonPropertyName("includes")]
        public Includes Includes { get; set; }

        [JsonPropertyName("matching_rules")]
        public List<MatchingRule> MatchingRules { get; set; }
    }

    public class Data
    {
        [JsonPropertyName("referenced_tweets")]
        public List<ReferencedTweet> ReferencedTweets { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("conversation_id")]
        public string ConversationId { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("author_id")]
        public string AuthorId { get; set; }
    }

    public class ReferencedTweet
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }

    public class User
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }
    }

    public class Includes
    {
        [JsonPropertyName("users")]
        public List<User> Users { get; set; }
    }

    public class MatchingRule
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("tag")]
        public string Tag { get; set; }
    }
}
