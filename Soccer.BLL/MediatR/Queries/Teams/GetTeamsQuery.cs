using MediatR;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Models;

namespace Soccer.BLL.MediatR.Queries.Teams
{
    public record GetTeamsQuery(TeamSearchModel SearchModel) : IRequest<PaginatedResponse<TeamVm>>;
}
