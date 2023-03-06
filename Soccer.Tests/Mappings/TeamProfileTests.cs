namespace Soccer.Tests.Mappings
{
    public class TeamProfileTests
    {
        private IMapper mapper = null!;
        public TeamProfileTests()
        {
            mapper = new MapperConfiguration(x => x.AddProfile(new TeamProfile())).CreateMapper();
        }
        
        [Fact(DisplayName = "TeamProfile Mapping Configuration Validation")]
        public void Test1_TeamProfileTests()
        {
            //Assert
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Theory(DisplayName = "Team Mapping Results Check")]
        [AutoData]
        public void Test2_TeamProfileTests(ResponseTeamImportDTO responseTeamImportDTO)
        {
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
    }
}
