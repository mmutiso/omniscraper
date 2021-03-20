using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.TwitterScraper
{
    public record BuildConversationResult
    {
        public bool Success { get; set; }
        public TwitterConversation TwitterConversation { get; init; }
    }
}
