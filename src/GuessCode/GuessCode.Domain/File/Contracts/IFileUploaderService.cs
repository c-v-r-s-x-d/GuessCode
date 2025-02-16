namespace GuessCode.Domain.File.Contracts;

public interface IFileUploaderService
{
    public Task<Guid> UploadFile(byte[] fileContent, CancellationToken cancellationToken);
}