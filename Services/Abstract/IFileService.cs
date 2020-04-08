using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstract
{
    public interface IFileService
    {
        List<string> RetrieveAll(string filePath);

        void Delete(List<string> filesToDelete);

        void DeleteDirectories(string directory);
    }
}
