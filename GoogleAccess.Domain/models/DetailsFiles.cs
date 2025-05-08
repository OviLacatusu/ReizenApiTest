namespace GoogleAccess.Domain.Models
{
    public class DetailsFiles{

        public IEnumerable<MediaFileDetails> mediaItems
        {
            get; set;
        } = new List<MediaFileDetails>();

        public string? nextPageToken
        {
            get; set;
        } = null;
        
    }
    public class MediaFileDetails
        {
            public string id { get; set; }
            public string productUrl { get; set; }
            public string filename { get; set; }
            public string baseUrl { get; set; }
        }
}
