using System.ComponentModel.DataAnnotations;
using GuessCode.DAL.Contexts;
using GuessCode.DAL.Models.Enums;
using GuessCode.DAL.Models.KataAggregate;
using GuessCode.Domain.Contracts;
using GuessCode.Domain.Scheduled.Models;
using GuessCode.Domain.Scheduled.Services;
using GuessCode.Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace GuessCode.Domain.Services;

public class KataCodeExecutionService : IKataCodeExecutionService
{
    private readonly GuessContext _context;
    private readonly CodeQueueService _codeQueueService;

    public KataCodeExecutionService(GuessContext context, CodeQueueService codeQueueService)
    {
        _context = context;
        _codeQueueService = codeQueueService;
    }

    public async Task ScheduleCodeExecution(long userId, long kataId, string sourceCode, CancellationToken cancellationToken)
    {
        var kata = await _context
            .Set<Kata>()
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == kataId, cancellationToken) ?? throw new ValidationException($"Kata {kataId} not found");
        
        var codeExecutionTask = new CodeExecutionTask
        {
            KataId = kata.Id,
            ExecutedBy = userId,
            Language = kata.ProgrammingLanguage.GetDescription(),
            SourceCode = sourceCode,
            Input = string.Empty // тут добавить поле для инпута тестов
        };

        await _codeQueueService.EnqueueAsync(codeExecutionTask);
    }
}