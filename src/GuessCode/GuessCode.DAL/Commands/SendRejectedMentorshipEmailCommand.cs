using MediatR;

namespace GuessCode.DAL.Commands;

public class SendRejectedMentorshipEmailCommand : BaseEmail, IRequest
{
}