using MediatR;
using Soccer.BLL.MediatR.Queries.Leagues;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Repositories.Interfaces;

namespace Soccer.BLL.MediatR.Handlers.Leagues
{
    public class GetLeagueByIdHandler : IRequestHandler<GetLeagueByIdQuery, LeagueVm>
    {
        private readonly ILeagueRepository _repository;
        private readonly IMapper _mapper;
        public GetLeagueByIdHandler(ILeagueRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<LeagueVm> Handle(
            GetLeagueByIdQuery request,
            CancellationToken cancellationToken) => _mapper.Map<LeagueVm>( await _repository.GetByIdAsync(request.Id));
    }
}
