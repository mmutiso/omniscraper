using System;
using System.Collections.Generic;

namespace Omniscraper.Core.TwitterScraper.Entities
{
	public class OpenAIResponse
    {
		public List<Choice> Choices { get; set; }
	}

	public class Choice
	{
		public string Text { get; set; }
		public string Finish_Reason { get; set; }
	}
}

