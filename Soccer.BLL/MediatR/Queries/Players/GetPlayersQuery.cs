using MediatR;
using Soccer.COMMON.ViewModels;

namespace Soccer.BLL.MediatR.Queries.Players
{
    public record GetPlayersQuery(PlayerSearchByParametersModel SearchModel) : IRequest<PaginatedResponse<PlayerVm>>;
}
