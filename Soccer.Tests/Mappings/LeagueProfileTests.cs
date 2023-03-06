namespace Soccer.Tests.Mappings
{
    public class LeagueProfileTests
    {
        private IMapper mapper = null!;
        public LeagueProfileTests()
        {
            mapper = new MapperConfiguration(x => x.AddProfile(new LeagueProfile())).CreateMapper();
        }

        [Fact(DisplayName = "LeagueProfile Mapping Configuration Validation")]
        public void Test1_LeagueProfileTests()
        {
            //Assert
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Theory(DisplayName = "League Mapping Results Check")]
        [AutoData]
        public void Test2_LeagueProfileTests(ResponseLeagueImportDTO responseLeagueImportDTO)
        {
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
