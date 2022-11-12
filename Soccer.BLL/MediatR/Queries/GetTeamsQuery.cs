using MediatR;
using Soccer.DAL.Models;

namespace Soccer.BLL.MediatR.Queries
{
    public record GetTeamsQuery() : IRequest<IEnumerable<Team>>;
}
