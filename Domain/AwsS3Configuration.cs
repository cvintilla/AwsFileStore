using Amazon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class AwsS3Configuration
    {
        public string BucketName { get; set; }

        public string KeyName { get; set; }

        public string AwsAccessKeyId { get; set; }

        public string AwsSecretAccessKey { get; set; }

        public string RegionEndpoint {get;set;}
    }
}
