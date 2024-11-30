using MediatR;

namespace GuessCode.Domain.Scheduled.Requests;

public class UpdateUserActivityStatusesCommand : IRequest
{
    public long[]? UserIds { get; set; }
}