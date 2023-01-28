using MediatR;
using Soccer.BLL.MediatR.Queries;
using Soccer.DAL.Models;
using Soccer.DAL.Repositories.Interfaces;

namespace Soccer.BLL.MediatR.Handlers.Teams
{
    public class GetTeamsHandler : IRequestHandler<GetTeamsQuery, IEnumerable<Team>>
    {
        private readonly ITeamRepository _repository;

        public GetTeamsHandler(ITeamRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Team>> Handle(
            GetTeamsQuery request,
            CancellationToken cancellationToken) => await _repository.GetAllAsync();
    }
}
