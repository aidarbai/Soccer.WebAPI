using Soccer.BLL.MediatR.Handlers.Teams;
using Soccer.BLL.MediatR.Queries.Teams;
using Soccer.DAL.Repositories.Interfaces;

namespace Soccer.Tests.MediatR.Handlers.Teams
{
    public class GetTeamByIdHandlerTests
    {
        private readonly Mock<ITeamRepository> repository;
        private readonly IMapper mapper;

        private readonly GetTeamByIdHandler sut;

        public GetTeamByIdHandlerTests()
        {
            repository = new();

            mapper = new MapperConfiguration(x =>x.AddProfile(new TeamMap())).CreateMapper();

            sut = new GetTeamByIdHandler(repository.Object, mapper);
        }

        [Theory(DisplayName = "Check If Handle Method Runs Correctly")]
        [AutoData]
        public async Task Test1_GetTeamByIdHandlerAsync(
            Team team,
            string teamId)
        {
            //Arrange
            repository.Setup(r => r.GetByIdAsync(teamId)).ReturnsAsync(team);

            //Act
            var response = await sut.Handle(new GetTeamByIdQuery(teamId), new CancellationToken());

            //Assert
            Assert.Equal(team.Name, response.Name);
        }
    }
}
