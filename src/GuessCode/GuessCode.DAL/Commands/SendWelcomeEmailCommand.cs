using MediatR;

namespace GuessCode.DAL.Commands;

public class SendWelcomeEmailCommand : BaseEmail, IRequest
{
}