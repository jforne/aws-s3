using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace POST_AOF_AWS_IMAGENES.Services
{
    public class ServiceS3
    {
        private string bucketName;
        private IAmazonS3 awsClient;

        public ServiceS3(IAmazonS3 client, IConfiguration configuration)
        {
            this.awsClient = client;
            this.bucketName = configuration.GetValue<string>("AWS:BucketName");
        }

        // Obtener archivos y carpetas dentro de un path específico
        public async Task<List<string>> GetFoldersAndFilesAsync(string prefix = "")
        {
            ListObjectsV2Request request = new ListObjectsV2Request
            {
                BucketName = this.bucketName,
                Prefix = prefix, // Filtrar elementos dentro de la carpeta
                Delimiter = "/"  // Permite distinguir carpetas de archivos
            };

            ListObjectsV2Response response = await this.awsClient.ListObjectsV2Async(request);

            List<string> items = new List<string>();

            // Agregar carpetas
            foreach (var folder in response.CommonPrefixes)
            {
                items.Add(folder); // Las carpetas terminan en "/"
            }

            // Agregar archivos
            foreach (var file in response.S3Objects)
            {
                items.Add(file.Key);
            }

            return items;
        }

        // Subir archivo dentro de una carpeta específica
        public async Task<bool> UploadFileAsync(Stream stream, string fileName, string folderPath = "")
        {
            // Limpiar la ruta de la carpeta para eliminar barras al final
            string cleanedFolderPath = folderPath?.TrimEnd('/') ?? "";

            // Si hay carpeta, agrega la barra solo si no está al final
            string fullPath = string.IsNullOrEmpty(cleanedFolderPath) ? fileName : $"{cleanedFolderPath}/{fileName}";

            PutObjectRequest request = new PutObjectRequest
            {
                InputStream = stream,
                Key = fullPath,
                BucketName = this.bucketName
            };
            PutObjectResponse response = await this.awsClient.PutObjectAsync(request);
            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}

