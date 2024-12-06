using System.ComponentModel.DataAnnotations;
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
    public class s3Controller : ControllerBase
    {
        [HttpGet("ListObjectsV2")]
        [Produces("application/json")]
        public async Task<List<oFile>> GetListObjectsV2([FromHeader][Required] string accessKey, [Required][FromHeader] string secretKey, [Required][FromHeader] string serviceUrl, [Required] string bucketName, [Required] string directory = "/", bool getInfo = true)
        {
            // Credenciales
            BasicAWSCredentials basicAwsCredentials = new BasicAWSCredentials(accessKey, secretKey);
            AmazonS3Client amazonS3Client = new AmazonS3Client(basicAwsCredentials, new AmazonS3Config
            {
                ServiceURL = $"https://{serviceUrl}",
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
                    BucketName = bucketName,
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
                        FileName = prefix,
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
                        S3Url = $"https://{bucketName}.{serviceUrl}/{directory}{fileInfo.Name}",
                        Key = $"{directory}{fileInfo.Name}",
                        MimeType = MimeTypeMap.GetMimeType(s3Object.Key)
                    });
                }

                // Actualizar el token para la siguiente iteración
                continuationToken = listObjectsV2Response.NextContinuationToken;
            } while (!string.IsNullOrEmpty(continuationToken));

            return result;
        }
    }
}
