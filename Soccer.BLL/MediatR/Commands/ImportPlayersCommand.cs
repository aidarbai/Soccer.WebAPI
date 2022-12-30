using MediatR;

namespace Soccer.BLL.MediatR.Commands
{
    public record ImportPlayersCommand(string Team) : IRequest;
}