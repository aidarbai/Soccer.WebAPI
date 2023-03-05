using MediatR;
using Soccer.BLL.MediatR.Queries.Leagues;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Helpers;
using Soccer.DAL.Repositories.Interfaces;

namespace Soccer.BLL.MediatR.Handlers.Leagues
{
    public class GetLeaguesHandler : IRequestHandler<GetLeaguesQuery, PaginatedResponse<LeagueVm>>
    {
        private readonly ILeagueRepository _repository;
        private readonly IMapper _mapper;

        public GetLeaguesHandler(ILeagueRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<LeagueVm>> Handle(
            GetLeaguesQuery request,
            CancellationToken cancellationToken)
        {
            var filter = LeagueFilterBuilder.Build(request.SearchModel);

            long count = await _repository.GetLeaguesQueryCountAsync(filter);

            int totalPages = (int)Math.Ceiling(decimal.Divide(count, request.SearchModel.PageSize));

            PaginatedResponse<LeagueVm> result;

            if (request.SearchModel.PageNumber > totalPages)
            {
                result = new PaginatedResponse<LeagueVm>
                {
                    PageSize = request.SearchModel.PageSize,
                    PageNumber = request.SearchModel.PageNumber + 1,
                    TotalPages = totalPages,
                    Results = new List<LeagueVm>()
                };

                return result;
            }

            var leagues = await _repository.GetLeaguesForPaginatedSearchResultsAsync(request.SearchModel, filter);

            result = new PaginatedResponse<LeagueVm>
            {
                ItemsCount = count,
                PageSize = request.SearchModel.PageSize,
                TotalPages = totalPages,
                PageNumber = request.SearchModel.PageNumber + 1,
                Results = _mapper.Map<List<LeagueVm>>(leagues)
            };

            return result;
        }
    }
}
