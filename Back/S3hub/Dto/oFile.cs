using System.Text.Json.Serialization;

namespace S3hub.Dto
{
    public class oFile
    {
        [JsonPropertyName("fileName")]
        public string FileName { get; set; }

        [JsonPropertyName("S3Url")]
        public string S3Url { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("mimeType")]
        public string MimeType{ get; set; }
    }
}
