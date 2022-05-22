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

        public async Task<List<string>> GetFilesAsync()
        {
            ListVersionsResponse response = await this.awsClient.ListVersionsAsync(this.bucketName);
            return response.Versions.Select(x => x.Key).ToList();
        }

        public async Task<bool> UploadFileAsync(Stream stream, string fileName)
        {
            PutObjectRequest request = new PutObjectRequest
            {
                InputStream = stream,
                Key = fileName,
                BucketName = this.bucketName
            };
            PutObjectResponse response = await this.awsClient.PutObjectAsync(request);
            if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
