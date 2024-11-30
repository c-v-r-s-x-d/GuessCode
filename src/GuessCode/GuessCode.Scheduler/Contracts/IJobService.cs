namespace GuessCode.Scheduler.Contracts;

public interface IJobService
{
    Task ExecuteJob(Type commandType);
}