using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace S3hub.Dto
{
    public class oAccount
    {
        [Required]
        [JsonPropertyName("accessKey")]
        public string AccessKey { set; get; }

        [Required]
        [JsonPropertyName("secretKey")]
        public string SecretKey { set; get; }

        [Required]
        [JsonPropertyName("serviceUrl")]
        public string ServiceUrl { set; get; }

        [Required]
        [JsonPropertyName("bucketName")]
        public string BucketName { set; get; }

        [JsonPropertyName("cdnUrl")]
        public string? CdnUrl { set; get; }
    }
}
