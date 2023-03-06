using MongoDB.Driver;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Repositories.Interfaces;
using Soccer.Tests.Customizations;

namespace Soccer.Tests.Services
{
    public class PlayerServiceTests
    {
        private readonly Mock<IPlayerRepository> repository;
        private readonly IMapper mapper;

        private readonly PlayerService sut;

        public PlayerServiceTests()
        {
            repository = new();

            mapper = new MapperConfiguration(x =>
            {
                x.AddProfile(new PlayerProfile());
                x.AddProfile(new StatisticProfile());
            }).CreateMapper();

            sut = new PlayerService(repository.Object, mapper);
        }

        [Theory(DisplayName = "Response Player Import Dto should return players list for league Id")]
        [AutoData]
        public void Test1_PlayerService(IEnumerable<ResponsePlayerImportDTO> responsePlayerImportDto)
        {
            //Arrange
            const int leagueId = 123;
            foreach (var item in responsePlayerImportDto)
            {
                item.Statistics.ForEach(x => x.League.Id = leagueId);
            }

            var playerDtos = responsePlayerImportDto.Select(x => x.Player);

            //Act
            var players = sut.MapPlayerDTOListToPlayerList(responsePlayerImportDto, leagueId.ToString());

            //Assert
            Assert.Equal(playerDtos.Count(), players.Count());

        }

        [Theory(DisplayName = "Response Player Import Dto should return empty list for non-existing league Id")]
        [AutoData]
        public void Test2_PlayerService(IEnumerable<ResponsePlayerImportDTO> responsePlayerImportDto)
        {
            //Arrange
            const int leagueId = 123;
            foreach (var item in responsePlayerImportDto)
            {
                item.Statistics.ForEach(x => x.League.Id = leagueId);
            }

            //Act
            const string nonExistingLeagueId = "124";
            var players = sut.MapPlayerDTOListToPlayerList(responsePlayerImportDto, nonExistingLeagueId);

            //Assert
            Assert.Empty(players);
        }
    }
}
