using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleAccess.Domain.Models
{
    public class GmailUserProfile
    {
        [JsonProperty ("emailAddress")]
        public string EmailAddress
        {
            get; set;
        }
        [JsonProperty ("messagesTotal")]
        public int MessagesTotal
        {
            get; set;
        }
        [JsonProperty ("threadsTotal")]
        public int ThreadsTotal
        {
            get; set;
        }
        [JsonProperty ("historyId")]
        public string HistoryId
        {
            get; set;
        }
    }
}
