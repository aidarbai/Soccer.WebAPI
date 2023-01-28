using MediatR;
using Soccer.BLL.MediatR.Queries;
using Soccer.DAL.Models;
using Soccer.DAL.Repositories.Interfaces;

namespace Soccer.BLL.MediatR.Handlers.Teams
{
    public class GetTeamByIdHandler : IRequestHandler<GetTeamByIdQuery, Team>
    {
        private readonly ITeamRepository _repository;

        public GetTeamByIdHandler(ITeamRepository repository)
        {
            _repository = repository;
        }

        public async Task<Team> Handle(
            GetTeamByIdQuery request,
            CancellationToken cancellationToken) => await _repository.GetByIdAsync(request.Id);
    }
}
