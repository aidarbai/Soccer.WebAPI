using AutoMapper;
using MediatR;
using Soccer.BLL.MediatR.Queries;
using Soccer.COMMON.Helpers;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Helpers;
using Soccer.DAL.Repositories.Interfaces;

namespace Soccer.BLL.MediatR.Handlers.Players
{
    public class GetPlayersHandler : IRequestHandler<GetPlayersQuery, PaginatedResponse<PlayerVM>>
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

        public async Task<PaginatedResponse<PlayerVM>> Handle(
            GetPlayersQuery request,
            CancellationToken cancellationToken)
        {
            DateHelperForSearchModel.ProcessAgeAndDates(request.SearchModel);

            var filter = FilterBuilder.Build(request.SearchModel);

            long count = await _repository.GetPlayersQueryCountAsync(filter);

            int totalPages = (int)Math.Ceiling(decimal.Divide(count, request.SearchModel.PageSize));

            if (request.SearchModel.PageNumber > totalPages)
            {
                return new PaginatedResponse<PlayerVM>
                {
                    PageSize = request.SearchModel.PageSize,
                    PageNumber = request.SearchModel.PageNumber + 1,
                    TotalPages = totalPages,
                };
            }

            var players = await _repository.GetPlayersForPaginatedSearchResultAsync(request.SearchModel, filter);

            var result = new PaginatedResponse<PlayerVM>
            {
                ItemsCount = count,
                PageSize = request.SearchModel.PageSize,
                TotalPages = totalPages,
                PageNumber = request.SearchModel.PageNumber + 1,
                Results = _mapper.Map<List<PlayerVM>>(players)
            };

            return result;
        }
    }
}
