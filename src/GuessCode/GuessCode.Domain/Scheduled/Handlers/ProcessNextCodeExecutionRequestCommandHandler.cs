using GuessCode.DAL.Contexts;
using GuessCode.DAL.Models.KataAggregate;
using GuessCode.DAL.Models.UserAggregate;
using GuessCode.DAL.Models.UserAggregate.Enums;
using GuessCode.Domain.Rating.Contracts;
using GuessCode.Domain.Scheduled.Requests;
using GuessCode.Domain.Scheduled.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace GuessCode.Domain.Scheduled.Handlers;

public class ProcessNextCodeExecutionRequestCommandHandler : IRequestHandler<ProcessNextCodeExecutionRequestCommand>
{
    private readonly CodeQueueService _queue;
    private readonly CodeExecutor _executor;
    
    private readonly IRatingService _ratingService;
    private readonly GuessContext _context;
    
    public ProcessNextCodeExecutionRequestCommandHandler(CodeQueueService queue, CodeExecutor executor, GuessContext context, IRatingService ratingService)
    {
        _queue = queue;
        _executor = executor;
        _context = context;
        _ratingService = ratingService;
    }

    public async Task Handle(ProcessNextCodeExecutionRequestCommand request, CancellationToken cancellationToken)
    {
        var currentTask = await _queue.DequeueAsync();
        if (currentTask is null) return;

        var result = await _executor.ExecuteAsync(currentTask);
        Console.WriteLine($"Task {currentTask.Id} result:\n{result}");

        var parsedResult = ParseExecutionResult(result);

        var codeExecutionResult = new KataCodeExecutionResult
        {
            ExecutedBy = currentTask.ExecutedBy,
            KataId = currentTask.KataId,
            Output = result,
            MemoryTaken = 0,
            TimeElapsed = parsedResult.Max(x => x.TimeElapsed),
            SourceCode = currentTask.SourceCode,
            TotalTestCount = parsedResult.Count(x => x.Passed),
            PassedTestCount = parsedResult.Count,

        };

        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        
        if (codeExecutionResult.TotalTestCount == codeExecutionResult.PassedTestCount)
        {
            var kata = await _context
                .Set<Kata>()
                .AsNoTracking()
                .SingleAsync(x => x.Id == currentTask.KataId, cancellationToken);
            
            await _ratingService.AdjustRatingAndUpdateRank(new RatingChange
            {
                UserId = currentTask.ExecutedBy,
                RatingChangeValue = kata.PointsReward,
                ChangeReason = RatingChangeReason.KataSolved,
                ChangedBy = SystemUserIds.System
            });

            var user = await _context.Set<User>()
                .Include(x => x.ResolvedKatas)
                .SingleAsync(x => x.Id == currentTask.ExecutedBy, cancellationToken);
            user.ResolvedKatas.Add(kata);
        }
        
        await _context.AddAsync(codeExecutionResult, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        
        await transaction.CommitAsync(cancellationToken);
    }

    private static List<ExecutionResult> ParseExecutionResult(string rawResult)
    {
        var testResults = rawResult.Split('\n');
        
        var results = new List<ExecutionResult>();

        foreach (var testResult in testResults)
        {
            if (testResult.IsNullOrEmpty())
            {
                continue;
            }
            var parts = testResult.Split();
            results.Add(new ExecutionResult
            {
                TestNumber = int.Parse(parts[0]),
                Passed = parts[1] == "PASSED",
                TimeElapsed = decimal.Parse(parts[2]),
                MemoryTaken = 0
            });
        }
        
        return results;
    }
    
    private class ExecutionResult
    {
        public int TestNumber { get; set; }
        
        public bool Passed { get; set; }
        
        public decimal TimeElapsed { get; set; }
        
        public decimal MemoryTaken { get; set; }
    }
}