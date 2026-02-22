namespace Application.Interfaces
{
    public interface IS3Storage
    {
        Task<(string objectKey, string? etag)> UploadAsync(
            Stream fileStream,
            string contentType,
            string objectKey,
            CancellationToken ct);

        string CreatePreSignedUrl(string objectKey, TimeSpan expiresIn);
        Task DeleteAsync(string objectKey, CancellationToken ct);
        string GetPublicUrl(string objectKey);
    }

}
