namespace Soccer.Tests.Mappings
{
    public class MapperConfigurationTests
    {
        private IMapper mapper = null!;
         
        [Fact(DisplayName = "PlayerMap Configuration Validation")]
        public void Test1_MapperConfigration_For_PlayerMap()
        {
            //Arrange
            mapper = new MapperConfiguration(x =>
            {
                x.AddProfile(new PlayerMap());
                x.AddProfile(new StatisticMap());
            }).CreateMapper();

            //Assert
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }
        [Theory(DisplayName = "Player Mapping Results Check")]
        [AutoData]
        public void Test2_MapperConfigration_For_PlayerMap(ResponsePlayerImportDTO responsePlayerImportDTO)
        {
            //Arrange
            mapper = new MapperConfiguration(x =>
            {
                x.AddProfile(new PlayerMap());
                x.AddProfile(new StatisticMap());
            }).CreateMapper();

            //Act
            var mappedPlayer = mapper.Map<Player>(responsePlayerImportDTO);

            //Assert
            Assert.Equal(responsePlayerImportDTO.Player.Id.ToString(), mappedPlayer.Id);
            Assert.Equal(responsePlayerImportDTO.Player.Name, mappedPlayer.Name);
            Assert.Equal(responsePlayerImportDTO.Player.Firstname, mappedPlayer.Firstname);
            Assert.Equal(responsePlayerImportDTO.Player.Lastname, mappedPlayer.Lastname);

            Assert.Equal(responsePlayerImportDTO.Player.Birth.Date, mappedPlayer.Birth.Date);
            Assert.Equal(responsePlayerImportDTO.Player.Birth.Place, mappedPlayer.Birth.Place);
            Assert.Equal(responsePlayerImportDTO.Player.Birth.Country, mappedPlayer.Birth.Country);
            
            Assert.Equal(responsePlayerImportDTO.Player.Nationality, mappedPlayer.Nationality);
            Assert.Equal(responsePlayerImportDTO.Player.Height, mappedPlayer.Height);
            Assert.Equal(responsePlayerImportDTO.Player.Weight, mappedPlayer.Weight);
            Assert.Equal(responsePlayerImportDTO.Player.Injured, mappedPlayer.Injured);
            Assert.Equal(responsePlayerImportDTO.Player.Photo, mappedPlayer.Photo);

            Assert.Equal(responsePlayerImportDTO.Statistics?.FirstOrDefault()?.Team.Id.ToString(), mappedPlayer.Statistics?.FirstOrDefault()?.Team);
            Assert.Equal(responsePlayerImportDTO.Statistics?.FirstOrDefault()?.League.Id.ToString(), mappedPlayer.Statistics?.FirstOrDefault()?.League);
            
        }

        [Fact(DisplayName = "TeamMap Configuration Validation")]
        public void Test3_MapperConfigration_For_TeamMap()
        {
            //Arrange
            mapper = new MapperConfiguration(x => x.AddProfile(new TeamMap())).CreateMapper();

            //Assert
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Theory(DisplayName = "Team Mapping Results Check")]
        [AutoData]
        public void Test4_MapperConfigration_For_TeamMap(ResponseTeamImportDTO responseTeamImportDTO)
        {
            //Arrange
            mapper = new MapperConfiguration(x => x.AddProfile(new TeamMap())).CreateMapper();

            //Act
            var mappedTeam = mapper.Map<Team>(responseTeamImportDTO);

            //Assert
            Assert.Equal(responseTeamImportDTO.Team.Id.ToString(), mappedTeam.Id);
            Assert.Equal(responseTeamImportDTO.Team.Name, mappedTeam.Name);
            Assert.Equal(responseTeamImportDTO.Team.Founded, mappedTeam.Founded);
            Assert.Equal(responseTeamImportDTO.Team.Logo, mappedTeam.Logo);
            Assert.Equal(responseTeamImportDTO.Venue.Name, mappedTeam.Venue);
            Assert.Equal(responseTeamImportDTO.Venue.City, mappedTeam.City);
        }

        [Fact(DisplayName = "LeagueMap Configuration Validation")]
        public void Test5_MapperConfigration_For_LeagueMap()
        {
            //Arrange
            mapper = new MapperConfiguration(x => x.AddProfile(new LeagueMap())).CreateMapper();

            //Assert
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Theory(DisplayName = "League Mapping Results Check")]
        [AutoData]
        public void Test6_MapperConfigration_For_TeamMap(ResponseLeagueImportDTO responseLeagueImportDTO)
        {
            //Arrange
            mapper = new MapperConfiguration(x => x.AddProfile(new LeagueMap())).CreateMapper();

            //Act
            var mappedLeague = mapper.Map<League>(responseLeagueImportDTO);

            //Assert
            Assert.Equal(responseLeagueImportDTO.League.Id.ToString(), mappedLeague.Id);
            Assert.Equal(responseLeagueImportDTO.League.Name, mappedLeague.Name);
            Assert.Equal(responseLeagueImportDTO.League.Logo, mappedLeague.Logo);
            Assert.Equal(responseLeagueImportDTO.Country.Name, mappedLeague.Country);
            Assert.Equal(responseLeagueImportDTO.Country.Flag, mappedLeague.Flag);
        }
    }
}
