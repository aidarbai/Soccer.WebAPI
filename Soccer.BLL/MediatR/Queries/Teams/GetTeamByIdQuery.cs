using MediatR;
using Soccer.COMMON.ViewModels;

namespace Soccer.BLL.MediatR.Queries.Teams
{
    public record GetTeamByIdQuery(string Id) : IRequest<TeamVm>;
}
