using MongoDB.Driver;
using Soccer.BLL.MediatR.Handlers.Teams;
using Soccer.BLL.MediatR.Queries.Teams;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Repositories.Interfaces;
using Soccer.Tests.Customizations;

namespace Soccer.Tests.MediatR.Handlers.Teams
{
    public class GetTeamsHandlerTests
    {
        private readonly Mock<ITeamRepository> repository;
        private readonly IMapper mapper;

        private readonly GetTeamsHandler sut;

        public GetTeamsHandlerTests()
        {
            repository = new();

            mapper = new MapperConfiguration(x =>x.AddProfile(new TeamProfile())).CreateMapper();

            sut = new GetTeamsHandler(repository.Object, mapper);
        }
        

        [Theory(DisplayName = "Handle Method Should Return Empty List in Results If Pagenumber Is More Than The Last Page")]
        [AutoData]
        public async Task Test1_GetTeamsHandlerAsync(List<Team> teamsList, TeamSearchModel searchModel)
        {
            //Arrange
            int totalRecords = teamsList.Count;
            searchModel.PageSize = 10;
            searchModel.PageNumber = (totalRecords / searchModel.PageSize) + 2;

            repository.Setup(r => r.GetTeamsQueryCountAsync(It.IsAny<FilterDefinition<Team>>()))
                .ReturnsAsync(totalRecords);

            //Act
            var response = await sut.Handle(new GetTeamsQuery(searchModel), new CancellationToken());

            //Assert
            Assert.Empty(response.Results);
        }

        [Theory(DisplayName = "Check If Handle Method Runs Correctly")]
        [PlayerSearchModelData]
        public async Task Test2_GetTeamsHandlerAsync(List<Team> teamsList, TeamSearchModel searchModel)
        {
            //Arrange
            searchModel.PageNumber = 0;
            searchModel.PageSize = 10;

            repository.Setup(r => r.GetTeamsQueryCountAsync(It.IsAny<FilterDefinition<Team>>()))
                .ReturnsAsync(teamsList.Count);
            repository.Setup(r => r.GetTeamsForPaginatedSearchResultsAsync(It.Is<TeamSearchModel>(x => x.PageSize == 10), It.IsAny<FilterDefinition<Team>>()))
                .ReturnsAsync(teamsList);

            //Act
            var response = await sut.Handle(new GetTeamsQuery(searchModel), new CancellationToken());

            //Assert
            Assert.Equivalent(teamsList, response.Results);
        }
    }
}
