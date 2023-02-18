using MediatR;
using Soccer.COMMON.ViewModels;

namespace Soccer.BLL.MediatR.Queries.Players
{
    public record GetPlayerByIdQuery(string Id) : IRequest<PlayerVm>;
}
