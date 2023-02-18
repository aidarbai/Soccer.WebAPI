using AutoMapper;
using MediatR;
using Soccer.BLL.MediatR.Queries.Players;
using Soccer.COMMON.Helpers;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Helpers;
using Soccer.DAL.Repositories.Interfaces;

namespace Soccer.BLL.MediatR.Handlers.Players
{
    public class GetPlayersHandler : IRequestHandler<GetPlayersQuery, PaginatedResponse<PlayerVm>>
    {
        private readonly IPlayerRepository _repository;
        private readonly IMapper _mapper;
        public GetPlayersHandler(
            IPlayerRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<PlayerVm>> Handle(
            GetPlayersQuery request,
            CancellationToken cancellationToken)
        {
            DateHelperForSearchModel.ProcessAgeAndDates(request.SearchModel);

            var filter = PlayerFilterBuilder.Build(request.SearchModel);

            long count = await _repository.GetPlayersQueryCountAsync(filter);

            int totalPages = (int)Math.Ceiling(decimal.Divide(count, request.SearchModel.PageSize));

            if (request.SearchModel.PageNumber > totalPages)
            {
                return new PaginatedResponse<PlayerVm>
                {
                    PageSize = request.SearchModel.PageSize,
                    PageNumber = request.SearchModel.PageNumber + 1,
                    TotalPages = totalPages,
                };
            }

            var players = await _repository.GetPlayersForPaginatedSearchResultAsync(request.SearchModel, filter);

            var result = new PaginatedResponse<PlayerVm>
            {
                ItemsCount = count,
                PageSize = request.SearchModel.PageSize,
                TotalPages = totalPages,
                PageNumber = request.SearchModel.PageNumber + 1,
                Results = _mapper.Map<List<PlayerVm>>(players)
            };

            return result;
        }
    }
}
