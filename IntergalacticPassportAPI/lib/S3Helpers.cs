using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Antiforgery;
using System;
using System.IO;
using System.Threading.Tasks;

namespace IntergalacticPassportAPI.lib.S3Helpers{
static class S3Helpers
{
    private const string bucketName = "spaceaffairsdocumentbucket";
    private static readonly RegionEndpoint bucketRegion = RegionEndpoint.AFSouth1;

    private static IAmazonS3 s3Client = new AmazonS3Client(Environment.GetEnvironmentVariable("AWS_SECRET_ID"), Environment.GetEnvironmentVariable("AWS_SECRET_KEY"), bucketRegion);

    public static async Task<String?> UploadFileAsync(IFormFile file, string filename)
    {
        try
        {   
            using (var stream = file.OpenReadStream()){
            var putRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = filename,
                InputStream = stream,
                ContentType = "application/pdf",
            };
            PutObjectResponse response = await s3Client.PutObjectAsync(putRequest);
            return $"https://{bucketName}.s3.{bucketRegion.SystemName}.amazonaws.com/{filename}";
            
            }
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine("S3 error: " + e.Message);
            return null;
        }
    }
}

}
