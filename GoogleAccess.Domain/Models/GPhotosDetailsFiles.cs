using Google.Apis.Admin.Directory.directory_v1.Data;
using Newtonsoft.Json;

namespace GoogleAccess.Domain.Models
{
    public class GPhotosDetailsFiles {
        [JsonProperty("mediaItems")]
        public IEnumerable<PickedMediaItem> MediaItems
        {
            get; set;
        } = new List<PickedMediaItem> ();
        [JsonProperty ("nextPageToken")]
        public string? NextPageToken
        {
            get; set;
        } = null;
    }
    public class MediaFile
    {
        [JsonProperty("mimeType")]
        public string MimeType { get; set; }
        [JsonProperty ("filename")]
        public string Filename { get; set; }
        [JsonProperty ("baseUrl")]
        public string BaseUrl { get; set; }
        [JsonProperty ("mediaFileMetadata")]
        public MediaFileMetadata MediaFileMetadata { get; set; }
    }
    public class PickedMediaItem
    {
        [JsonProperty ("id")]
        public string Id
        {
            get; set;
        }
        [JsonProperty ("createTime")]
        public string CreateTime
        {
            get; set;
        }
        [JsonProperty ("type")]
        public string Type
        {
            get; set;
        }
        [JsonProperty ("mediaFile")]
        public MediaFile MediaFile
        {
            get; set;
        }
        
    }
    public class MediaFileMetadata
    {
        [JsonProperty ("width")]
        public int Width { get; set; }
        [JsonProperty ("height")]
        public int Height { get; set; }
        [JsonProperty ("cameraMake")]
        public string CameraMake { get; set; }
        [JsonProperty ("cameraModel")]
        public string CameraModel { get; set; }
        [JsonProperty ("photoMetadata")]
        public PhotoMetadata PhotoMetadata { get; set; }
        [JsonProperty ("videoMetadata")]
        public VideoMetadata VideoMetadata { get; set; }
    }
    public class PhotoMetadata
    {
        [JsonProperty ("focalLength")]
        public decimal FocalLength { get; set; }
        [JsonProperty ("aperturefNumber")]
        public decimal AperturefNumber { get; set; }
        [JsonProperty ("isoEquivalent")]
        public int IsoEquivalent { get; set; }
        [JsonProperty ("exposureTime")]
        public string ExposureTime { get; set; }
    }
    public class VideoMetadata
    {
        [JsonProperty ("fps")]
        public decimal Fps { get; set; }
        [JsonProperty ("processingStatus")]
        public string ProcessingStatus { get; set; }
    }
    public enum Type { TYPE_UNSPECIFIED, PHOTO, VIDEO };

    public enum VideoProcessingStatus { UNSCPECIFIED, PROCESSING, READY, FAILED };
}
