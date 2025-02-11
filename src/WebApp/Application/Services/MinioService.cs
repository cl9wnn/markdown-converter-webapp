using System.Text;
using Application.Models;
using Core.Interfaces;
using Core.Interfaces.Services;
using Core.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
namespace Application.Services;

public class MinioService: IS3Service
{
    private const string DefaultContentType = "text/markdown";
    private const string BucketName = "md-bucket";

    private readonly MinIoSettings _minIoSettings;
    private readonly IMinioClient _minioClient;
    private readonly ILogger<MinioService> _logger;

    public MinioService(IOptions<MinIoSettings> options, ILogger<MinioService> logger)
    {
        _logger = logger;
        _minIoSettings = options.Value;

        _minioClient = new MinioClient()
            .WithEndpoint(_minIoSettings.Endpoint)
            .WithCredentials(_minIoSettings.AccessKey, _minIoSettings.SecretKey)
            .Build();
    }

    public async Task<Result<string>> UploadMarkdownTextAsync(string markdownContent, string objectName, CancellationToken cancellationToken)
    {
        var fileBytes = Encoding.UTF8.GetBytes(markdownContent);
        var resultString = await UploadFileAsync(fileBytes, objectName, DefaultContentType, cancellationToken);

        return resultString != null 
            ? Result<string>.Success(resultString) 
            : Result<string>.Failure("Ошибка загрузки Markdown-файла.")!;
    }
    
    public async Task<bool> DeleteFileAsync(string objectName, CancellationToken cancellationToken = default)
    {
        try
        {
            await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(BucketName)
                .WithObject(objectName), cancellationToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError("Ошибка при удалении файла {ObjectName}: {ErrorMessage}", objectName, ex.Message);
            return false;
        }
    }

    public async Task<string?> GetMarkdownTextAsync(string objectName, CancellationToken cancellationToken = default)
    {
        try
        {
            var fileBytes = await GetFileAsync(objectName, cancellationToken);
            return Encoding.UTF8.GetString(fileBytes);
        }
        catch (Exception ex)
        {
            _logger.LogError("Ошибка при получении файла: {ErrorMessage}", ex.Message);
            return null;
        }
    }
    
    private async Task EnsureBucketExistsAsync()
    {
        var found = await _minioClient.BucketExistsAsync(
            new BucketExistsArgs().WithBucket(BucketName)
        );
        
        if (!found)
        {
            await _minioClient.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(BucketName)
            );
        }
    }

    private async Task<string?> UploadFileAsync(byte[] fileBytes, string objectName, string contentType = DefaultContentType, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureBucketExistsAsync();

            using var stream = new MemoryStream(fileBytes);
            await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(BucketName)
                .WithObject(objectName)
                .WithStreamData(stream)
                .WithObjectSize(fileBytes.Length)
                .WithContentType(contentType), cancellationToken);

            var objectUrl = $"{_minIoSettings.Endpoint}/{BucketName}/{objectName}";
            return objectUrl;
        }
        catch (Exception ex)
        {
            _logger.LogError("Ошибка при загрузке файла: {ErrorMessage}", ex.Message);
            return null;
        }
    }

    private async Task<byte[]?> GetFileAsync(string objectName, CancellationToken cancellationToken = default)
    {
        try
        {
            using var memoryStream = new MemoryStream();
            await _minioClient.GetObjectAsync(new GetObjectArgs()
                .WithBucket(BucketName)
                .WithObject(objectName)
                .WithCallbackStream(async stream =>
                {
                    await stream.CopyToAsync(memoryStream, cancellationToken);
                }), cancellationToken);

            return memoryStream.ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError("Ошибка при получении файла: {ErrorMessage}", ex.Message);
            return null;
        }
    }
}