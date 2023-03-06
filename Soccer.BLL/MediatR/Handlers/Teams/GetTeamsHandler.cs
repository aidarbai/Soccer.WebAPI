using MediatR;
using Soccer.BLL.MediatR.Queries.Teams;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Helpers;
using Soccer.DAL.Models;
using Soccer.DAL.Repositories.Interfaces;

namespace Soccer.BLL.MediatR.Handlers.Teams
{
    public class GetTeamsHandler : IRequestHandler<GetTeamsQuery, PaginatedResponse<TeamVm>>
    {
        private readonly ITeamRepository _repository;
        private readonly IMapper _mapper;
        public GetTeamsHandler(ITeamRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<PaginatedResponse<TeamVm>> Handle(
            GetTeamsQuery request,
            CancellationToken cancellationToken)
        {
            var filter = TeamFilterBuilder.Build(request.SearchModel);

            long count = await _repository.GetTeamsQueryCountAsync(filter);

            int totalPages = (int)Math.Ceiling(decimal.Divide(count, request.SearchModel.PageSize));

            PaginatedResponse<TeamVm> result;

            if (request.SearchModel.PageNumber > totalPages)
            {
                result = new PaginatedResponse<TeamVm>
                {
                    PageSize = request.SearchModel.PageSize,
                    PageNumber = request.SearchModel.PageNumber + 1,
                    TotalPages = totalPages,
                    Results = new List<TeamVm>()
                };
                return result;
            }

            var teams = await _repository.GetTeamsForPaginatedSearchResultsAsync(request.SearchModel, filter);

            result = new PaginatedResponse<TeamVm>
            {
                ItemsCount = count,
                PageSize = request.SearchModel.PageSize,
                TotalPages = totalPages,
                PageNumber = request.SearchModel.PageNumber + 1,
                Results = _mapper.Map<List<TeamVm>>(teams)
            };

            return result;
        }
    }
}
