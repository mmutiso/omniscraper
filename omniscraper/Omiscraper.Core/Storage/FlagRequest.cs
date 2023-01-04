using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omniscraper.Core.Storage
{
    public enum RequestStatus 
    {
        Pending,
        Approved,
        Rejected
    }
    public class FlagRequest
    {
        public Guid Id { get; set; }
        public string Slug { get; set; }
        public string TwitterHandle { get; set; }
        public DateTime DateRequested { get; set; }
        public DateTime DateOfAction { get; set; }
        public string FlaggingReason { get; set; }
        public RequestStatus RequestStatus { get; set; }
    }
}
