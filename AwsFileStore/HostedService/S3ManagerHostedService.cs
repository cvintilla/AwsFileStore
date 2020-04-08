using Domain;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Serilog;
using Services.Abstract;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AwsFileStore
{
    public class S3ManagerHostedService : IHostedService
    {
        private static UploadFileConfiguration _uploadConfig;
        private readonly IFileService _fileService;
        private readonly IAwsService _awsService;

        public S3ManagerHostedService(
            IOptions<UploadFileConfiguration> uploadConfig,
            IFileService fileService,
            IAwsService awsService)
        {
            _uploadConfig = uploadConfig.Value;
            _fileService = fileService;
            _awsService = awsService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                // retrieves all files from a directory
                var filesRetrieved = _fileService.RetrieveAll(_uploadConfig.Directory);

                // upload the files to s3
                var filesToDelete =  await _awsService.UploadAsync(filesRetrieved);

                // delete the files 
                _fileService.Delete(filesToDelete);

                // delete the left over directories
                if (filesRetrieved.Count == filesToDelete.Count)
                    _fileService.DeleteDirectories(_uploadConfig.Directory);

                Log.Information($"Sleeping for: {_uploadConfig.UploadInterval}.");

                Thread.Sleep(_uploadConfig.UploadInterval);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Log.Debug("Application shutting down.");

            return Task.CompletedTask;
        }
    }
}
