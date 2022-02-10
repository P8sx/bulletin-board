using BulletinBoard.Model;
using Microsoft.AspNetCore.Components.Forms;
using Minio;

namespace BulletinBoard.Services;

public class GlobalService
{
    

    #region Globals
    public Guid DefaultBoardGuid { get; }
    public int MaxImagesPerBulletin { get; }
    public int MaxFileSize { get; }
    public string DefaultImageFolder { get; }
    public enum State
    {
        Loading,
        Success,
        AccessBlocked
    }
    #endregion

    #region Privates
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _env;
    private readonly MinioClient _minioClient;
    private readonly bool _minioStorage;
    private readonly string _bucket;
    private readonly string _serviceUrl;

    #endregion
    public GlobalService(IConfiguration configuration, IWebHostEnvironment env, MinioClient minioClient)
    {
        _configuration = configuration;
        _env = env;
        _minioClient = minioClient;
        DefaultBoardGuid = Guid.Parse(configuration["DefaultBoardId"]);
        DefaultImageFolder = configuration["DefaultFolder"];
        
        MaxImagesPerBulletin = int.TryParse(configuration["MaxImagesPerBulletin"], out var result) ? result : 10;
        MaxFileSize = int.TryParse(configuration["MaxFileSizeMB"], out var result2) ? result2 * 1024 * 1024 : 5 * (1024 * 1024);
        _minioStorage = bool.TryParse(configuration["MinioStorage"], out var result3) && result3;
        _serviceUrl= configuration["Minio:ServiceURL"];
        _bucket = configuration["Minio:Bucket"];

    }
    

    public string AvatarImage(Image? img) => img == null ? $"/avatar.png" : Path(img);
    public string BoardImage(Image? img) => img == null ? $"/board.svg" : Path(img);
    public string BulletinImage(Image? img) => img == null ? "#" : Path(img);
    private string Path(Image img) => _minioStorage ? $"https://{_serviceUrl}/{_bucket}/{img.FileName()}" : $"{DefaultImageFolder}/{img.FileName()}";

    
    public async Task<bool> SaveFilesAsync(IBrowserFile browserFile, Image image)
    {
        switch (_minioStorage)
        {
            case false:
            {
                var stream = browserFile.OpenReadStream(MaxFileSize);
                var folder = $"{_env.WebRootPath}/{DefaultImageFolder}";
                if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
                var fs = File.Create($"{folder}/{image.Guid}.{image.FileExtension}");
                await stream.CopyToAsync(fs);
                stream.Close();
                fs.Close();
                return true;
            }
            case true:
            {
                var stream = browserFile.OpenReadStream(MaxFileSize);
                await _minioClient.PutObjectAsync(_bucket, image.FileName(),stream, stream.Length);
            }
            break;
        }

        return false;
    }
}