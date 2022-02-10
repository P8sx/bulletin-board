using BulletinBoard.Model;
using Microsoft.AspNetCore.Components.Forms;

namespace BulletinBoard.Services;

public class GlobalService
{
    

    #region Globals
    public Guid DefaultBoardId { get; }
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
    private readonly bool _s3Storage;

    #endregion
    public GlobalService(IConfiguration configuration, IWebHostEnvironment env)
    {
        _configuration = configuration;
        _env = env;
        DefaultBoardId = Guid.Parse(configuration["DefaultBoardId"]);
        DefaultImageFolder = configuration["DefaultFolder"];
        
        MaxImagesPerBulletin = int.TryParse(configuration["MaxImagesPerBulletin"], out var result) ? result : 10;
        MaxFileSize = int.TryParse(configuration["MaxFileSizeMB"], out var result2) ? result2 * 1024 ^ 2 : 5 * 1024 ^ 2;
        _s3Storage = bool.TryParse(configuration["S3Storage"], out var result3);
        
    }
    

    public string AvatarImage(Image? img) => img == null ? $"/avatar.png" : img.FileName();
    public string BoardImage(Image? img) => img == null ? $"/board.svg": img.FileName();
    public string BulletinImage(Image? img)
    {
        return "test";
    }

    
    
    public async Task<bool> SaveFilesAsync(IBrowserFile browserFile, Image image)
    {
        var stream = browserFile.OpenReadStream(MaxFileSize);
        var folder = $"{_env.WebRootPath}/{DefaultImageFolder}";
        if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
        var fs = File.Create($"{_env.WebRootPath}/{BulletinImage(image)}");
        await stream.CopyToAsync(fs);
        stream.Close();
        fs.Close();
        return true;
    }
}