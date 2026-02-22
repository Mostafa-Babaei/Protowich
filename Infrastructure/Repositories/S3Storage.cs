using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Application.Features.S3.Dtos;
using Application.Interfaces;
using Microsoft.Extensions.Options;

namespace Infrastructure.Repositories
{
    public class S3Storage : IS3Storage
    {
        private readonly S3Options _opt;
        private readonly IAmazonS3 _s3;

        public S3Storage(IOptions<S3Options> opt)
        {
            _opt = opt.Value;

            var creds = new BasicAWSCredentials(_opt.AccessKey, _opt.SecretKey);

            var cfg = new AmazonS3Config
            {
                ServiceURL = _opt.ServiceUrl,
                ForcePathStyle = _opt.ForcePathStyle,
                UseHttp = !_opt.UseSsl
            };

            _s3 = new AmazonS3Client(creds, cfg);
        }

        public async Task<(string objectKey, string? etag)> UploadAsync(
            Stream fileStream,
            string contentType,
            string objectKey,
            CancellationToken ct)
        {
            var request = new Amazon.S3.Model.PutObjectRequest
            {
                BucketName = _opt.Bucket,
                Key = NormalizeKey(objectKey),
                InputStream = fileStream,
                ContentType = contentType,
                CannedACL = S3CannedACL.PublicRead
            };

            var response = await _s3.PutObjectAsync(request, ct);
            return (objectKey, response.ETag);
        }
        public string GetPublicUrl(string objectKey)
        {
            var baseUrl = _opt.ServiceUrl.TrimEnd('/');
            var encodedKey = string.Join("/",
                objectKey.Split('/')
                         .Select(Uri.EscapeDataString));

            return $"{baseUrl}/{_opt.Bucket}/{_opt.FolderPrefix}/{encodedKey}";
        }


        public string CreatePreSignedUrl(string objectKey, TimeSpan expiresIn)
        {
            var request = new GetPreSignedUrlRequest
            {
                BucketName = _opt.Bucket,
                Key = objectKey,
                Expires = DateTime.UtcNow.Add(expiresIn),
                Verb = HttpVerb.GET
            };

            return _s3.GetPreSignedURL(request);
        }
        private string NormalizeKey(string objectKey)
        {
            var prefix = (_opt.FolderPrefix ?? "").Trim().Trim('/');
            if (string.IsNullOrEmpty(prefix)) return objectKey.TrimStart('/');
            return $"{prefix}/{objectKey.TrimStart('/')}";
        }
        public async Task DeleteAsync(string objectKey, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(objectKey))
                return;

            var key = NormalizeKey(objectKey); // همون متدی که قبلاً داشتیم

            var request = new Amazon.S3.Model.DeleteObjectRequest
            {
                BucketName = _opt.Bucket,
                Key = key
            };

            await _s3.DeleteObjectAsync(request, ct);
        }


    }
}
