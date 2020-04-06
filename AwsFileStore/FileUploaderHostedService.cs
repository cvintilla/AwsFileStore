using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AwsFileStore
{
    public class FileUploaderHostedService : IHostedService
    {
        private static IAmazonS3 s3Client;

        private readonly IConfiguration _configuration;
        private static UploadFileConfiguration _uploadConfig;
        private static AwsS3Configuration _awsConfig;

        public FileUploaderHostedService(IConfiguration configuration, 
            IOptions<UploadFileConfiguration> uploadConfig, 
            IOptions<AwsS3Configuration> awsConfig)
        {
            _configuration = configuration;
            _uploadConfig = uploadConfig.Value;
            _awsConfig = awsConfig.Value;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var regionEndpoint = RegionEndpoint.GetBySystemName(_awsConfig.RegionEndpoint);

            s3Client = new AmazonS3Client(_awsConfig.AwsAccessKeyId, _awsConfig.AwsSecretAccessKey, regionEndpoint);

            UploadFileAsync().Wait();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Debug("Application shutting down.");

            return Task.CompletedTask;
        }

        private async Task UploadFileAsync()
        {
            try
            {
                var fileTransferUtility =
                    new TransferUtility(s3Client);

                // Option 1. Upload a file. The file name is used as the object key name.
                //await fileTransferUtility.UploadAsync(filePath, bucketName);
                //Console.WriteLine("Upload 1 completed");

                // Option 2. Specify object key name explicitly.
                await fileTransferUtility.UploadAsync(_uploadConfig.FileLocation, _awsConfig.BucketName, _awsConfig.KeyName);
                Console.WriteLine("Upload 2 completed");

                // Option 3. Upload data from a type of System.IO.Stream.
                using (var fileToUpload =
                    new FileStream(_uploadConfig.FileLocation, FileMode.Open, FileAccess.Read))
                {
                    await fileTransferUtility.UploadAsync(fileToUpload,
                                               _awsConfig.BucketName, _awsConfig.KeyName);
                }
                Console.WriteLine("Upload 3 completed");

                // Option 4. Specify advanced settings.
                var fileTransferUtilityRequest = new TransferUtilityUploadRequest
                {
                    BucketName = _awsConfig.BucketName,
                    FilePath = _uploadConfig.FileLocation,
                    StorageClass = S3StorageClass.StandardInfrequentAccess,
                    PartSize = 6291456, // 6 MB.
                    Key = _awsConfig.KeyName,
                    CannedACL = S3CannedACL.PublicRead
                };
                fileTransferUtilityRequest.Metadata.Add("param1", "Value1");
                fileTransferUtilityRequest.Metadata.Add("param2", "Value2");

                await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
                Console.WriteLine("Upload 4 completed");
            }
            catch (AmazonS3Exception e)
            {
                Log.Error($"Error encountered on server. Message:'{e.Message}' when writing an object");
            }
            catch (Exception e)
            {
                Log.Error($"Unknown encountered on server.Message:'{e.Message}' when writing an object");
            }
        }
    }
}
