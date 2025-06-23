using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoogleAccess.Domain.Models
{
    public class GmailMessage
    {
        [JsonPropertyName ("id")]
        public string? Id { get; set; }
        [JsonPropertyName ("threadId")]
        public string? ThreadId { get; set; }
        [JsonPropertyName ("labelIds")]
        public string[]? LabelIds { get; set; }
        [JsonPropertyName ("snippet")]
        public string? Snippet { get; set; }
        [JsonPropertyName ("historyId")]
        public string? HistoryId { get; set; }
        [JsonPropertyName ("internalDate")]
        public string? InternalDate { get; set; }
        [JsonPropertyName ("payload")]
        public MessagePart? Payload { get; set; }
        [JsonPropertyName ("sizeEstimate")]
        public int? SizeEstimate { get; set; }
        [JsonPropertyName ("raw")]
        public string? Raw { get; set; }
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
