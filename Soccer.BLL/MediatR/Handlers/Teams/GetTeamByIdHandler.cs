using MediatR;
using Soccer.BLL.MediatR.Queries.Teams;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Repositories.Interfaces;

namespace Soccer.BLL.MediatR.Handlers.Teams
{
    public class GetTeamByIdHandler : IRequestHandler<GetTeamByIdQuery, TeamVm>
    {
        private readonly ITeamRepository _repository;
        private readonly IMapper _mapper;

        public GetTeamByIdHandler(ITeamRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TeamVm> Handle(
            GetTeamByIdQuery request,
            CancellationToken cancellationToken) => _mapper.Map<TeamVm>(await _repository.GetByIdAsync(request.Id));
    }
}
