using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Omniscraper.Api.Models
{
    public class ThreadRequestModel
    {
        [Required]
        public string ConversationId { get; set; }
        [Required]
        public string AuthorId { get; set; }
    }
}
