using GuessCode.Domain.File.Contracts;

namespace GuessCode.Domain.File.Services;

public class FileUploaderService : IFileUploaderService
{
    private const string UploadStaticFolder = @"/app/wwwroot";
    private const string UploadLocalFolder = @"/app/local";
    
    public async Task<Guid> UploadFile(byte[] fileContent, string fileExtension, bool uploadInStaticFolder, CancellationToken cancellationToken)
    {
        EnsureDirectoryExists(uploadInStaticFolder ? UploadStaticFolder : UploadLocalFolder);
        
        var fileId = Guid.NewGuid();
        var fileName = $"{fileId}{fileExtension}";
        var filePath = Path.Combine(uploadInStaticFolder ? UploadStaticFolder : UploadLocalFolder, fileName);
        
        await System.IO.File.WriteAllBytesAsync(filePath, fileContent, cancellationToken);

        return fileId;
    }

    private static void EnsureDirectoryExists(string folder)
    {
        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }
    }
}