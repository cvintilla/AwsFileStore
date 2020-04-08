using Serilog;
using Services.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Services
{
    public class FileService : IFileService
    {
        public List<string> RetrieveAll(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException("FilePath cannot be null or empty.");

            var files = Directory.GetFiles(filePath, "*", SearchOption.AllDirectories).ToList();

            Log.Information($"File(s) retrieved from directory: {files.Count}.");

            return files;
        }

        public void Delete(List<string> filesToDelete)
        {
            foreach (var file in filesToDelete)
            {
                try
                {
                    File.Delete(file);

                    Log.Information($"Deleted: {file}");
                }
                catch (Exception ex)
                {
                    Log.Error($"Error occurred while deleting: {ex.Message}");
                }
            }
        }

        public void DeleteDirectories(string directory)
        {
            var directories = Directory.GetDirectories(directory);

            foreach (var dir in directories)
            {
                // dont delete directory if there's a missed file in it
                if (Directory.GetFiles(directory).Count() == 0)
                {
                    Directory.Delete(dir);
                }
            }
        }
    }
}
