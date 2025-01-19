using System.Text;
using Application.Models;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
namespace Application.Services;

public class MinioService
{
    private const string DefaultContentType = "text/markdown";

    private readonly MinIoSettings _minIoSettings;
    private readonly IMinioClient _minioClient;

    public MinioService(IOptions<MinIoSettings> options)
    {
        _minIoSettings = options.Value;

        _minioClient = new MinioClient()
            .WithEndpoint(_minIoSettings.Endpoint)
            .WithCredentials(_minIoSettings.AccessKey, _minIoSettings.SecretKey)
            .Build();
    }

    private async Task EnsureBucketExistsAsync()
    {
        var found = await _minioClient.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(_minIoSettings.BucketName)
        );
        if (!found)
        {
            await _minioClient.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(_minIoSettings.BucketName)
            );
        }
    }

    public async Task<string?> UploadMarkdownTextAsync(string markdownContent, string objectName, CancellationToken cancellationToken)
    {
        var fileBytes = Encoding.UTF8.GetBytes(markdownContent);
        return await UploadFileAsync(fileBytes, objectName, DefaultContentType, cancellationToken);
    }

    public async Task<string?> UploadFileAsync(byte[] fileBytes, string objectName, string contentType = DefaultContentType, CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureBucketExistsAsync();

            using var stream = new MemoryStream(fileBytes);
            await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(_minIoSettings.BucketName)
                .WithObject(objectName)
                .WithStreamData(stream)
                .WithObjectSize(fileBytes.Length)
                .WithContentType(contentType), cancellationToken);

            var objectUrl = $"{_minIoSettings.Endpoint}/{_minIoSettings.BucketName}/{objectName}";
            return objectUrl;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при загрузке файла: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> DeleteFileAsync(string objectName, CancellationToken cancellationToken = default)
    {
        try
        {
            await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(_minIoSettings.BucketName)
                .WithObject(objectName), cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при удалении файла {objectName}: {ex.Message}");
            return false;
        }
    }

    public async Task<string?> GetMarkdownTextAsync(string objectName, CancellationToken cancellationToken = default)
    {
        try
        {
            var fileBytes = await GetFileAsync(objectName, cancellationToken);
            return fileBytes != null ? Encoding.UTF8.GetString(fileBytes) : null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при получении файла: {ex.Message}");
            return null;
        }
    }

    public async Task<byte[]> GetFileAsync(string objectName, CancellationToken cancellationToken = default)
    {
        try
        {
            using var memoryStream = new MemoryStream();
            await _minioClient.GetObjectAsync(new GetObjectArgs()
                .WithBucket(_minIoSettings.BucketName)
                .WithObject(objectName)
                .WithCallbackStream(async stream =>
                {
                    await stream.CopyToAsync(memoryStream, cancellationToken);
                }), cancellationToken);

            return memoryStream.ToArray();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при получении файла {objectName}: {ex.Message}");
            return null;
        }
    }
}