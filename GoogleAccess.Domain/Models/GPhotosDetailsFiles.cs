using Google.Apis.Admin.Directory.directory_v1.Data;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GoogleAccess.Domain.Models
{
    public class GPhotosDetailsFiles {
        [JsonPropertyName("mediaItems")]
        public IEnumerable<PickedMediaItem> MediaItems
        {
            get; set;
        } = new List<PickedMediaItem> ();
        [JsonPropertyName ("nextPageToken")]
        public string? NextPageToken
        {
            get; set;
        } = null;
    }
    public class MediaFile
    {
        [JsonPropertyName("mimeType")]
        public string? MimeType { get; set; }
        [JsonPropertyName ("filename")]
        public string? Filename { get; set; }
        [JsonPropertyName ("baseUrl")]
        public string? BaseUrl { get; set; }
        [JsonPropertyName ("mediaFileMetadata")]
        public MediaFileMetadata? MediaFileMetadata { get; set; }
    }
    public class PickedMediaItem
    {
        [JsonPropertyName ("id")]
        public string? Id
        {
            get; set;
        }
        [JsonPropertyName ("createTime")]
        public string? CreateTime
        {
            get; set;
        }
        [JsonPropertyName ("type")]
        public string? Type
        {
            get; set;
        }
        [JsonPropertyName ("mediaFile")]
        public MediaFile? MediaFile
        {
            get; set;
        }
        
    }
    public class MediaFileMetadata
    {
        [JsonPropertyName ("width")]
        public int Width { get; set; }
        [JsonPropertyName ("height")]
        public int Height { get; set; }
        [JsonPropertyName ("cameraMake")]
        public string? CameraMake { get; set; }
        [JsonPropertyName ("cameraModel")]
        public string? CameraModel { get; set; }
        [JsonPropertyName ("photoMetadata")]
        public PhotoMetadata? PhotoMetadata { get; set; }
        [JsonPropertyName ("videoMetadata")]
        public VideoMetadata? VideoMetadata { get; set; }
    }
    public class PhotoMetadata
    {
        [JsonPropertyName ("focalLength")]
        public decimal FocalLength { get; set; }
        [JsonPropertyName ("aperturefNumber")]
        public decimal AperturefNumber { get; set; }
        [JsonPropertyName ("isoEquivalent")]
        public int IsoEquivalent { get; set; }
        [JsonPropertyName ("exposureTime")]
        public string? ExposureTime { get; set; }
    }
    public class VideoMetadata
    {
        [JsonPropertyName ("fps")]
        public decimal Fps { get; set; }
        [JsonPropertyName ("processingStatus")]
        public string? ProcessingStatus { get; set; }
    }
    public enum Type { TYPE_UNSPECIFIED, PHOTO, VIDEO };

    public enum VideoProcessingStatus { UNSCPECIFIED, PROCESSING, READY, FAILED };
}
