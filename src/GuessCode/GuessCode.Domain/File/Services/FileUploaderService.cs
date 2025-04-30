using GuessCode.Domain.File.Contracts;

namespace GuessCode.Domain.File.Services;

public class FileUploaderService : IFileUploaderService
{
    private const string UploadFolder = @"/app/wwwroot";
    
    public async Task<Guid> UploadFile(byte[] fileContent, string fileExtension, CancellationToken cancellationToken)
    {
        EnsureDirectoryExists();
        
        var fileId = Guid.NewGuid();
        var fileName = $"{fileId}{fileExtension}";
        var filePath = Path.Combine(UploadFolder, fileName);
        
        await System.IO.File.WriteAllBytesAsync(filePath, fileContent, cancellationToken);

        return fileId;
    }

    private static void EnsureDirectoryExists()
    {
        if (!Directory.Exists(UploadFolder))
        {
            Directory.CreateDirectory(UploadFolder);
        }
    }
}