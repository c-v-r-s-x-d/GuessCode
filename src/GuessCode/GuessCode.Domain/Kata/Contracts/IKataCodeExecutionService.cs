namespace GuessCode.Domain.Contracts;

public interface IKataCodeExecutionService
{
    Task ScheduleCodeExecution(long userId, long kataId, string sourceCode, CancellationToken cancellationToken);
}