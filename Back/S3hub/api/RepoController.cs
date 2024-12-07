using System.ComponentModel.DataAnnotations;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Mvc;
using MimeTypes;
using S3hub.Dto;

namespace S3hub.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepoController : ControllerBase
    {
        [HttpPost("ListObjectsV2")]
        [Produces("application/json")]
        public async Task<List<oFile>> PostListObjectsV2([FromBody] oAccount oAccount, [Required] string directory = "/", bool getInfo = true)
        {
            // Credenciales
            BasicAWSCredentials basicAwsCredentials = new BasicAWSCredentials(oAccount.AccessKey, oAccount.SecretKey);
            AmazonS3Client amazonS3Client = new AmazonS3Client(basicAwsCredentials, new AmazonS3Config
            {
                ServiceURL = $"https://{oAccount.ServiceUrl}",
            });

            // Resultado
            string continuationToken = null;
            List<oFile> result = new List<oFile>();
            directory = directory == "/" ? "" : directory;

            do
            {
                // Solicitud para listar objetos
                ListObjectsV2Request listObjectsV2Request = new ListObjectsV2Request
                {
                    BucketName = oAccount.BucketName,
                    Prefix = directory, // Indica el "directorio" que quieres listar
                    Delimiter = "/", // Esto separa los "directorios" y sus contenidos
                    ContinuationToken = continuationToken,
                    //MaxKeys = 1000,
                };

                // Llamada al servicio
                ListObjectsV2Response listObjectsV2Response = await amazonS3Client.ListObjectsV2Async(listObjectsV2Request);

                // Agregar los "subdirectorios" al resultado
                foreach (var prefix in listObjectsV2Response.CommonPrefixes)
                {
                    result.Add(new oFile
                    {
                        FileName = directory != "" ? prefix.Replace(directory, string.Empty) : prefix,
                        S3Url = prefix,
                        Key = prefix,
                        MimeType = "/"
                    });
                }

                // Agregar los archivos al resultado
                foreach (var s3Object in listObjectsV2Response.S3Objects)
                {
                    // Saltamos el primero
                    if (s3Object.Key == directory)
                        continue;

                    FileInfo fileInfo = new FileInfo(s3Object.Key);
                    result.Add(new oFile
                    {
                        FileName = fileInfo.Name,
                        S3Url = $"https://{oAccount.BucketName}.{oAccount.ServiceUrl}/{directory}{fileInfo.Name}",
                        Key = $"{directory}{fileInfo.Name}",
                        MimeType = MimeTypeMap.GetMimeType(s3Object.Key)
                    });
                }

                // Actualizar el token para la siguiente iteración
                continuationToken = listObjectsV2Response.NextContinuationToken;
            } while (!string.IsNullOrEmpty(continuationToken));

            return result;
        }

        [HttpPost("PreSignedUrl")]
        [Produces("application/json")]
        public string PostPreSignedUrl([FromBody] oAccount oAccount, [Required] string key, int minExp = 5)
        {
            // Credenciales
            BasicAWSCredentials basicAwsCredentials = new BasicAWSCredentials(oAccount.AccessKey, oAccount.SecretKey);
            AmazonS3Client amazonS3Client = new AmazonS3Client(basicAwsCredentials, new AmazonS3Config
            {
                ServiceURL = $"https://{oAccount.ServiceUrl}",
            });

            // Cremos la url
            AWSConfigsS3.UseSignatureVersion4 = true;
            GetPreSignedUrlRequest getPreSignedUrlRequest = new GetPreSignedUrlRequest
            {
                BucketName = oAccount.BucketName,
                Key = key,
                Verb = HttpVerb.GET,
                Expires = DateTime.UtcNow.AddMinutes(Math.Abs(minExp)),
            };

            // Obtenemos
            string urlString = amazonS3Client.GetPreSignedURL(getPreSignedUrlRequest);

            // CDN
            if (string.IsNullOrWhiteSpace(oAccount.CdnUrl) == false)
                urlString = urlString.Replace($"https://{oAccount.BucketName}.{oAccount.ServiceUrl}/", $"https://{oAccount.CdnUrl}/");

            // Devolvemos
            return urlString;
        }
    }
}
