namespace GuessCode.Domain.File.Contracts;

public interface IFileUploaderService
{
    public Task<Guid> UploadFile(byte[] fileContent, string fileExtension, bool uploadInStaticFolder, CancellationToken cancellationToken);
}