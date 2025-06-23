
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GoogleAccess.Domain.Models
{
    public class GmailUserProfile
    {
        [JsonPropertyName ("emailAddress")]
        public string? EmailAddress
        {
            get; set;
        }
        [JsonPropertyName ("messagesTotal")]
        public int? MessagesTotal
        {
            get; set;
        }
        [JsonPropertyName ("threadsTotal")]
        public int? ThreadsTotal
        {
            get; set;
        }
        [JsonPropertyName ("historyId")]
        public string? HistoryId
        {
            get; set;
        }
    }
}
