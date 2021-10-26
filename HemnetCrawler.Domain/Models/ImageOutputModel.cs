
namespace HemnetCrawler.Domain.Models
{
    public class ImageOutputModel
    {
        public byte[] Data { get; }
        public string ContentType { get; }

        public ImageOutputModel(byte[] data, string contentType)
        {
            Data = data;
            ContentType = contentType;
        }
    }
}
