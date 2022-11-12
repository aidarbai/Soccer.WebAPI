using MediatR;
using Soccer.DAL.Models;

namespace Soccer.BLL.MediatR.Commands
{
    public record AddTeamCommand(Team Team) : IRequest<Team>;
}
