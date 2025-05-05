using System.ComponentModel.DataAnnotations;
using GuessCode.DAL.Models.RoleAggregate;
using GuessCode.Domain.File.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GuessCode.API.Controllers;

[ApiController]
[Authorize(Roles = $"{RoleNameConstants.Admin}, {RoleNameConstants.User}")]
[Route("api/file")]
public class FileController : BaseGuessController
{
    private readonly IFileUploaderService _fileUploaderService;

    public FileController(IFileUploaderService fileUploaderService)
    {
        _fileUploaderService = fileUploaderService;
    }

    [HttpPost]
    public async Task<Guid> UploadFile(IFormFile? file, bool uploadInStaticFolder, CancellationToken cancellationToken)
    {
        if (file is null || file.Length == 0)
        {
            throw new ValidationException("File is empty");
        }
        
        using var memoryStream = new MemoryStream();
        var fileExtension = Path.GetExtension(file.FileName);
        await file.CopyToAsync(memoryStream, cancellationToken);
        var fileBytes = memoryStream.ToArray();
        
        return await _fileUploaderService.UploadFile(fileBytes, fileExtension, uploadInStaticFolder, cancellationToken);
    }
}