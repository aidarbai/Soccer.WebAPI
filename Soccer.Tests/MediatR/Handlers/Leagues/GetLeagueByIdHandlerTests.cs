using Soccer.BLL.MediatR.Handlers.Leagues;
using Soccer.BLL.MediatR.Queries.Leagues;
using Soccer.DAL.Repositories.Interfaces;

namespace Soccer.Tests.MediatR.Handlers.Leagues
{
    public class GetLeagueByIdHandlerTests
    {
        private readonly Mock<ILeagueRepository> repository;
        private readonly IMapper mapper;

        private readonly GetLeagueByIdHandler sut;
        public GetLeagueByIdHandlerTests()
        {
            repository = new();

            mapper = new MapperConfiguration(x => x.AddProfile(new LeagueProfile())).CreateMapper();

            sut = new GetLeagueByIdHandler(repository.Object, mapper);
        }

        [Theory(DisplayName = "Check If Handle Method Runs Correctly")]
        [AutoData]
        public async Task Test1_GetLeagueByIdHandlerAsync(
            League league,
            string leagueId)
        {
            //Arrange
            repository.Setup(r => r.GetByIdAsync(leagueId)).ReturnsAsync(league);

            //Act
            var response = await sut.Handle(new GetLeagueByIdQuery(leagueId), new CancellationToken());

            //Assert
            Assert.Equal(league.Name, response.Name);
        }
    }
}
