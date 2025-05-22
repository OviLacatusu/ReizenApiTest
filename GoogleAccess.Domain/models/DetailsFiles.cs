using Google.Apis.Admin.Directory.directory_v1.Data;

namespace GoogleAccess.Domain.Models
{
    public class DetailsFiles {

        public IEnumerable<PickedMediaItem> mediaItems
        {
            get; set;
        } = new List<PickedMediaItem> ();

        public string? nextPageToken
        {
            get; set;
        } = null;
    }
    public class MediaFile
    {
        public string mimeType { get; set; }
        public string filename { get; set; }
        public string baseUrl { get; set; }
        public MediaFileMetadata mediaFileMetadata { get; set; }
    }
    public class PickedMediaItem
    {
        public string id
        {
            get; set;
        }
        public string createTime
        {
            get; set;
        }
        public string type
        {
            get; set;
        }
        public MediaFile mediaFile
        {
            get; set;
        }
        
    }
    public class MediaFileMetadata
    {
        public int width { get; set; }
        public int height { get; set; }
        public string cameraMake { get; set; }
        public string cameraModel { get; set; }
        public PhotoMetadata photoMetadata { get; set; }
        public VideoMetadata videoMetadata { get; set; }
    }
    public class PhotoMetadata
    {
        public decimal focalLength { get; set; }
        public decimal aperturefNumber { get; set; }
        public int isoEquivalent { get; set; }
        public string exposureTime { get; set; }
    }
    public class VideoMetadata
    {
        public decimal fps { get; set; }
        public string processingStatus { get; set; }
    }
    public enum Type { TYPE_UNSPECIFIED, PHOTO, VIDEO };

    public enum VideoProcessingStatus { UNSCPECIFIED, PROCESSING, READY, FAILED };
}
