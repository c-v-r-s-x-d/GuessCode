using GuessCode.Domain.File.Contracts;

namespace GuessCode.Domain.File.Services;

public class FileUploaderService : IFileUploaderService
{
    public Task<Guid> UploadFile(byte[] fileContent, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}