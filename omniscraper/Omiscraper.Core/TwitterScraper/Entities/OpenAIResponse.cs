using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Omniscraper.Core.TwitterScraper.Entities
{
	public class OpenAIResponse
    {
		[JsonPropertyName("choices")]
		public List<Choice> Choices { get; set; }
	}

	public class Choice
	{
		[JsonPropertyName("text")]
		public string Text { get; set; }
		[JsonPropertyName("finish_reason")]
		public string Finish_Reason { get; set; }
	}
}

