using MongoDB.Driver;
using Soccer.BLL.MediatR.Handlers.Leagues;
using Soccer.BLL.MediatR.Queries.Leagues;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Repositories.Interfaces;
using Soccer.Tests.Customizations;

namespace Soccer.Tests.MediatR.Handlers.Leagues
{
    public class GetLeaguesHandlerTests
    {
        private readonly Mock<ILeagueRepository> repository;
        private readonly IMapper mapper;

        private readonly GetLeaguesHandler sut;

        public GetLeaguesHandlerTests()
        {
            repository = new();

            mapper = new MapperConfiguration(x =>x.AddProfile(new LeagueProfile())).CreateMapper();

            sut = new GetLeaguesHandler(repository.Object, mapper);
        }
        

        [Theory(DisplayName = "Handle Method Should Return Empty List in Results If Pagenumber Is More Than The Last Page")]
        [AutoData]
        public async Task Test1_GetLeaguesHandlerAsync(List<League> leaguesList, LeagueSearchModel searchModel)
        {
            //Arrange
            int totalRecords = leaguesList.Count;
            searchModel.PageSize = 10;
            searchModel.PageNumber = (totalRecords / searchModel.PageSize) + 2;

            repository.Setup(r => r.GetLeaguesQueryCountAsync(It.IsAny<FilterDefinition<League>>()))
                .ReturnsAsync(totalRecords);

            //Act
            var response = await sut.Handle(new GetLeaguesQuery(searchModel), new CancellationToken());

            //Assert
            Assert.Empty(response.Results);
        }

        [Theory(DisplayName = "Check If Handle Method Runs Correctly")]
        [PlayerSearchModelData]
        public async Task Test2_GetLeaguesHandlerAsync(List<League> leaguesList, LeagueSearchModel searchModel)
        {
            //Arrange
            searchModel.PageNumber = 0;
            searchModel.PageSize = 10;

            repository.Setup(r => r.GetLeaguesQueryCountAsync(It.IsAny<FilterDefinition<League>>()))
                .ReturnsAsync(leaguesList.Count);
            repository.Setup(r => r.GetLeaguesForPaginatedSearchResultsAsync(It.IsAny<LeagueSearchModel>(), It.IsAny<FilterDefinition<League>>()))
                .ReturnsAsync(leaguesList);

            //Act
            var response = await sut.Handle(new GetLeaguesQuery(searchModel), new CancellationToken());

            //Assert
            Assert.Equivalent(leaguesList, response.Results);
        }
    }
}
