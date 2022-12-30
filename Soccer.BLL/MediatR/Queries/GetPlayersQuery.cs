using MediatR;
using Soccer.COMMON.ViewModels;

namespace Soccer.BLL.MediatR.Queries
{
    public record GetPlayersQuery(PlayerSearchByParametersModel SearchModel) : IRequest<PaginatedResponse<PlayerVM>>;
}
