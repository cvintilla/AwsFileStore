using System;

namespace Domain
{
    public class UploadFileConfiguration
    {
        public string Directory { get; set; }

        public TimeSpan UploadInterval { get; set; }
}
}
