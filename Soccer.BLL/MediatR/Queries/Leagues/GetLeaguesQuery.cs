using MediatR;
using Soccer.COMMON.ViewModels;

namespace Soccer.BLL.MediatR.Queries.Leagues
{
    public record GetLeaguesQuery(LeagueSearchModel SearchModel) : IRequest<PaginatedResponse<LeagueVm>>;
    
}
