using AutoMapper;
using MediatR;
using Soccer.BLL.MediatR.Queries;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Repositories.Interfaces;

namespace Soccer.BLL.MediatR.Handlers.Players
{
    public class GetPlayerByIdHandler : IRequestHandler<GetPlayerByIdQuery, PlayerVM>
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

        public async Task<PlayerVM> Handle(
            GetPlayerByIdQuery request,
            CancellationToken cancellationToken) => _mapper.Map<PlayerVM>(await _repository.GetByIdAsync(request.Id));
    }
}
