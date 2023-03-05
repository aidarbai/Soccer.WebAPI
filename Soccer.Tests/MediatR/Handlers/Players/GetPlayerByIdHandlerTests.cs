using Soccer.BLL.MediatR.Handlers.Players;
using Soccer.BLL.MediatR.Queries.Players;
using Soccer.DAL.Repositories.Interfaces;

namespace Soccer.Tests.MediatR.Handlers.Players
{
    public class GetPlayerByIdHandlerTests
    {
        private readonly Mock<IPlayerRepository> repository;
        private readonly IMapper mapper;

        private readonly GetPlayerByIdHandler sut;
        public GetPlayerByIdHandlerTests()
        {
            repository = new();

            mapper = new MapperConfiguration(x =>
            {
                x.AddProfile(new PlayerMap());
                x.AddProfile(new StatisticMap());
            }).CreateMapper();

            sut = new GetPlayerByIdHandler(repository.Object, mapper);
        }

        [Theory(DisplayName = "Check If Handle Method Runs Correctly")]
        [AutoData]
        public async Task Test1_GetPlayerByIdHandlerAsync(
            Player player,
            string playerId)
        {
            //Arrange
            repository.Setup(r => r.GetByIdAsync(playerId))
                .ReturnsAsync(player);

            //Act
            var response = await sut.Handle(new GetPlayerByIdQuery(playerId), new CancellationToken());

            //Assert
            Assert.Equal(player.Name, response.Name);
        }
    }
}
