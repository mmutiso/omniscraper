using Omniscraper.Core.TwitterScraper;
using Omniscraper.Core.TwitterScraper.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Omniscraper.Core.TwitterScraper.Entities.v2;

namespace Omniscraper.Core.TwitterScraper.ContentHandlers
{
    public interface ITweetContentHandler
    {
        ITweetContentHandler SetNext(ITweetContentHandler handler);

        Task HandleAsync<T>(StreamedTweetContent notification, ILogger<T> logger);
    }
}
