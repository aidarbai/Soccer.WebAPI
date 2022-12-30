using MediatR;
using Soccer.COMMON.ViewModels;

namespace Soccer.BLL.MediatR.Queries
{
    public record GetPlayerByIdQuery(string Id) : IRequest<PlayerVM>;
}
