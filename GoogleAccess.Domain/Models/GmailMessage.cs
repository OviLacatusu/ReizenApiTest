using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleAccess.Domain.Models
{
    public class GmailMessage
    {
        [JsonProperty ("id")]
        public string Id { get; set; }
        [JsonProperty ("threadId")]
        public string ThreadId { get; set; }
        [JsonProperty ("labelIds")]
        public string[] LabelIds { get; set; }
        [JsonProperty ("snippet")]
        public string Snippet { get; set; }
        [JsonProperty ("historyId")]
        public string HistoryId { get; set; }
        [JsonProperty ("internalDate")]
        public string InternalDate { get; set; }
        [JsonProperty ("payload")]
        public MessagePart Payload { get; set; }
        [JsonProperty ("sizeEstimate")]
        public int SizeEstimate { get; set; }
        [JsonProperty ("raw")]
        public string Raw { get; set; }
    }

    public record MessagePart(
        string partId,
        string mimeType,
        string filename,
        Header[] headers,
        MessagePartBody body,
        MessagePart[] parts
        );

    public record MessagePartBody(
        string attachmentId,
        int size,
        string data // base64 encoded?
    );
    public record Header(
        string name,
        string value
        );
}
