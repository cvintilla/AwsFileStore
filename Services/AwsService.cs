using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Domain;
using Microsoft.Extensions.Options;
using Serilog;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class AwsService : IAwsService
    {
        private readonly AwsS3Configuration _awsConfig;
        private static IAmazonS3 s3Client;

        public AwsService(IOptions<AwsS3Configuration> awsConfig)
        {
            _awsConfig = awsConfig.Value;
        }

        public async Task<List<string>> UploadAsync(List<string> files)
        {
            var filesToDelete = new List<string>();
            
            var regionEndpoint = RegionEndpoint.GetBySystemName(_awsConfig.RegionEndpoint);

            s3Client = new AmazonS3Client(_awsConfig.AwsAccessKeyId, _awsConfig.AwsSecretAccessKey, regionEndpoint);

            var fileTransferUtility = new TransferUtility(s3Client);

            foreach (var file in files)
            {
                try
                {
                    await fileTransferUtility.UploadAsync(file, _awsConfig.BucketName);

                    Log.Debug($"Successfully Uploaded: {file}");

                    filesToDelete.Add(file);
                }
                catch (AmazonS3Exception e)
                {
                    Log.Error($"Error encountered uploading {file} to s3. Message:'{e.Message}'");
                }
                catch (Exception e)
                {
                    Log.Error($"Error encountered uploading {file} to s3. Message:'{e.Message}'");
                }
            }

            return filesToDelete;
        }
    }
}
