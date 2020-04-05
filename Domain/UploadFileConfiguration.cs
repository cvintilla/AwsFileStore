using System;

namespace Domain
{
    public class UploadFileConfiguration
    {
        public string FileLocation { get; set; }

        public TimeSpan UploadInterval { get; set; }
    }
}
