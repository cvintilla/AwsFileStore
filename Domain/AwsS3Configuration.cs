namespace Domain
{
    public class AwsS3Configuration
    {
        public string BucketName { get; set; }

        public string AwsAccessKeyId { get; set; }

        public string AwsSecretAccessKey { get; set; }

        public string RegionEndpoint {get;set;}
    }
}
