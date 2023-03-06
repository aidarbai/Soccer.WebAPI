namespace Soccer.Tests.Mappings
{
    public class PlayerProfileTests
    {
        private IMapper mapper = null!;
        public PlayerProfileTests()
        {
            mapper = new MapperConfiguration(x =>
            {
                x.AddProfile(new PlayerProfile());
                x.AddProfile(new StatisticProfile());
            }).CreateMapper();
        }

        [Fact(DisplayName = "PlayerProfile Mapping Configuration Validation")]
        public void Test1_PlayerProfileTests()
        {
            //Assert
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Theory(DisplayName = "Player Mapping Results Check")]
        [AutoData]
        public void Test2_PlayerProfileTests(ResponsePlayerImportDTO responsePlayerImportDTO)
        {
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

            Assert.Equal(responsePlayerImportDTO.Statistics.First().Team.Id.ToString(), mappedPlayer.Statistics.First().Team);
            Assert.Equal(responsePlayerImportDTO.Statistics.First().League.Id.ToString(), mappedPlayer.Statistics.First().League);

        }
    }
}
