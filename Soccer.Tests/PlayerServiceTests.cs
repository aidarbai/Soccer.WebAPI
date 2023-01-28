using AutoFixture.Xunit2;
using AutoMapper;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using MongoDB.Driver;
using Moq;
using Soccer.BLL.DTOs;
using Soccer.BLL.Mappings;
using Soccer.BLL.Services;
using Soccer.COMMON.ViewModels;
using Soccer.DAL.Models;
using Soccer.DAL.Repositories.Interfaces;
using Soccer.Tests.Customizations;
using System.Reflection;
using System.Xml.Linq;
using Xunit.Abstractions;

namespace Soccer.Tests
{
    public class PlayerServiceTests
    {
        private readonly Mock<IPlayerRepository> repository;
        //private readonly Mock<IMapper> mapper;
        private readonly IMapper mapper;

        private readonly PlayerService sut;

        public PlayerServiceTests()
        {
            repository = new();
            //mapper = new();

            mapper = new MapperConfiguration(x =>
            {
                x.AddProfile(new PlayerMap());
                x.AddProfile(new StatisticMap());
            }).CreateMapper();

            //sut = new PlayerService(repository.Object, mapper.Object);
            sut = new PlayerService(repository.Object, mapper);
        }

        [Theory(DisplayName = "SearchByParametersAsync Should Return Null Results If Pagenumber IsMore Than Total Pages")]
        [AutoData]
        public async Task Test1_PlayerServiceAsync(PlayerSearchByParametersModel searchModel)
        {
            //Arrange
            searchModel.PageNumber = 999;

            //Act
            var response = await sut.SearchByParametersAsync(searchModel);

            //Assert
            Assert.Null(response.Results);
        }

        [Theory(DisplayName = "SearchByParametersAsync Should Not Return Null Results If Pagernumber Is Less Than Total Pages")]
        [PlayerSearchModelData]
        public async Task Test2_PlayerServiceAsync(
            List<Player> playersList,
            //List<PlayerVM> playersVMList,
            PlayerSearchByParametersModel searchModel)
        {
            //Arrange
            repository.Setup(r => r.GetPlayersQueryCountAsync(It.IsAny<FilterDefinition<Player>>()))
                .ReturnsAsync(10);
            repository.Setup(r => r.GetPlayersForPaginatedSearchResultAsync(It.IsAny<PlayerSearchByParametersModel>(), It.IsAny<FilterDefinition<Player>>()))
                .ReturnsAsync(playersList);

            //mapper.Setup(x => x.Map<List<PlayerVM>>(It.IsAny<List<Player>>()))
            //    .Returns(playersVMList);

            //Action act = () => sut.SearchByParametersAsync(searchModel);
            //TODO check action exception

            //Act
            var response = await sut.SearchByParametersAsync(searchModel);

            //Assert
            Assert.True(response.Results.Count > 0);
            Assert.Equivalent(playersList, response.Results);

            //TODO cover all code with tests, use exclude attribute
            //TODO create tests for mapper configuration
        }

        [Theory(DisplayName = "Player list should be mapped correctly to Player VM list")]
        [PlayerSearchModelData]
        public async Task Test3_PlayerServiceAsync(
            List<Player> playersList,
            PlayerSearchByParametersModel searchModel)
        {
            //Arrange
            repository.Setup(r => r.GetPlayersQueryCountAsync(It.IsAny<FilterDefinition<Player>>()))
                .ReturnsAsync(10);
            repository.Setup(r => r.GetPlayersForPaginatedSearchResultAsync(It.IsAny<PlayerSearchByParametersModel>(), It.IsAny<FilterDefinition<Player>>()))
                .ReturnsAsync(playersList);

            //Act
            var response = await sut.SearchByParametersAsync(searchModel);
            //response.Results.RemoveAt(0);

            //Assert
            Assert.Equivalent(playersList, response.Results);
        }

        [Theory(DisplayName = "Response Pleayer Import Dto should be mapped correctly to Player list")]
        [AutoData]
        public void Test4_PlayerService(IEnumerable<ResponsePlayerImportDTO> responsePlayerImportDto)
        {
            //Arrange

            const int leagueId = 123;
            foreach (var item in responsePlayerImportDto)
            {
                item.Statistics.ForEach(x => x.League.Id = leagueId);
            }

            var playerDtos = responsePlayerImportDto.Select(x => x.Player).ToList();

            //Act
            var players = sut.MapPlayerDTOListToPlayerList(responsePlayerImportDto, leagueId.ToString()).ToList();

            //Assert
            Assert.Equal(playerDtos.Count, players.Count);

            //Assert.Equivalent(playerDtos, players);

            //players[0].Id = "";

            for (int i = 0; i < playerDtos.Count; i++)
            {
                foreach (var item in playerDtos[i].GetType().GetProperties())
                {
                    var expectedValue = item.GetValue(playerDtos[i]);
                    string propName = item.Name;

                    var prop = players[i].GetType().GetProperty(propName);
                    var actualValue = prop?.GetValue(players[i]);

                    if (expectedValue?.GetType() != actualValue?.GetType())
                    {
                        expectedValue = expectedValue?.ToString();
                        actualValue = actualValue?.ToString();
                    }

                    Assert.Equal(expectedValue, actualValue);
                }
            }
        }
    }
}
