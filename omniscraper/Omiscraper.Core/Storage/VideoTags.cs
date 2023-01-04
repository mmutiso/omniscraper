using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.Storage
{
    public class VideoTag
    {
        public Guid Id { get; set; }
        public string TagName { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public DateTime DateCreated { get; set; }

        public ICollection<TwitterVideo> TwitterVideos { get; set; }
    }
}
