using MediatR;
using Soccer.COMMON.ViewModels;

namespace Soccer.BLL.MediatR.Queries.Leagues
{
    public record GetLeagueByIdQuery(string Id) : IRequest<LeagueVm>;
}
