using MongoDB.Driver;
using Soccer.BLL.MediatR.Handlers.Players;
using Soccer.BLL.MediatR.Queries.Players;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Repositories.Interfaces;
using Soccer.Tests.Customizations;

namespace Soccer.Tests.MediatR.Handlers.Players
{
    public class GetPlayersHandlerTests
    {
        private readonly Mock<IPlayerRepository> repository;
        private readonly IMapper mapper;

        private readonly GetPlayersHandler sut;

        public GetPlayersHandlerTests()
        {
            repository = new();

            mapper = new MapperConfiguration(x =>
            {
                x.AddProfile(new PlayerMap());
                x.AddProfile(new StatisticMap());
            }).CreateMapper();

            sut = new GetPlayersHandler(repository.Object, mapper);
        }
        

        [Theory(DisplayName = "Handle Method Should Return Empty List in Results If Pagenumber Is More Than The Last Page")]
        [PlayerSearchModelData]
        public async Task Test1_GetPlayersHandlerAsync(List<Player> playersList, PlayerSearchByParametersModel searchModel)
        {
            //Arrange
            int totalRecords = playersList.Count;
            searchModel.PageSize = 10;
            searchModel.PageNumber = (totalRecords / searchModel.PageSize) + 2;

            repository.Setup(r => r.GetPlayersQueryCountAsync(It.IsAny<FilterDefinition<Player>>()))
                .ReturnsAsync(totalRecords);

            //Act
            var response = await sut.Handle(new GetPlayersQuery(searchModel), new CancellationToken());

            //Assert
            Assert.Empty(response.Results);
        }

        [Theory(DisplayName = "Check If Handle Method Runs Correctly")]
        [PlayerSearchModelData]
        public async Task Test2_GetPlayersHandlerAsync(List<Player> playersList, PlayerSearchByParametersModel searchModel)
        {
            //Arrange
            searchModel.PageNumber = 0;
            searchModel.PageSize = 10;

            repository.Setup(r => r.GetPlayersQueryCountAsync(It.IsAny<FilterDefinition<Player>>()))
                .ReturnsAsync(playersList.Count);
            repository.Setup(r => r.GetPlayersForPaginatedSearchResultAsync(It.IsAny<PlayerSearchByParametersModel>(), It.IsAny<FilterDefinition<Player>>()))
                .ReturnsAsync(playersList);

            //Act
            var response = await sut.Handle(new GetPlayersQuery(searchModel), new CancellationToken());

            //Assert
            Assert.Equivalent(playersList, response.Results);
        }
    }
}
