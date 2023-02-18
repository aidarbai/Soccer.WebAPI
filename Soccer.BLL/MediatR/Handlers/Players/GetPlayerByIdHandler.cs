using AutoMapper;
using MediatR;
using Soccer.BLL.MediatR.Queries.Players;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Repositories.Interfaces;

namespace Soccer.BLL.MediatR.Handlers.Players
{
    public class GetPlayerByIdHandler : IRequestHandler<GetPlayerByIdQuery, PlayerVm>
    {
        private readonly IPlayerRepository _repository;
        private readonly IMapper _mapper;
        public GetPlayerByIdHandler(
            IPlayerRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PlayerVm> Handle(
            GetPlayerByIdQuery request,
            CancellationToken cancellationToken) => _mapper.Map<PlayerVm>(await _repository.GetByIdAsync(request.Id));
    }
}
