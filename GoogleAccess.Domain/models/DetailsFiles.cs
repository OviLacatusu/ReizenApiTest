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
    public class MediaFileDetails
    {
        public string mimeType { get; set; }
        //public string productUrl { get; set; }
        public string filename { get; set; }
        public string baseUrl { get; set; }
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
        public type type;
        public MediaFileDetails mediaFile
        {
            get; set;
        }
    }
    public enum type { TYPE_UNSPECIFIED, PHOTO, VIDEO };
}
