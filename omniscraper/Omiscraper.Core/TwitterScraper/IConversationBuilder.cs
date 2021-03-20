using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.TwitterScraper
{
    public interface IConversationBuilder
    {
        Task<BuildConversationResult> BuildAsync(long authorId, long conversationId);
    }
}
