namespace Soccer.Tests.Mappings
{
    public class StatisticProfileTests
    {
        private IMapper mapper = null!;
        public StatisticProfileTests()
        {
            mapper = new MapperConfiguration(x => x.AddProfile(new StatisticProfile())).CreateMapper();
        }

        [Fact(DisplayName = "StatisticProfile Mapping Configuration Validation")]
        public void Test1_StatisticProfileTests()
        {
            //Assert
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        [Theory(DisplayName = "Statistic Mapping Results Check")]
        [AutoData]
        public void Test2_StatisticProfileTests(StatisticImportDTO statisticImportDTO)
        {
            //Act
            var mappedStatistic = mapper.Map<Statistic>(statisticImportDTO);

            //Assert
            Assert.Equal(statisticImportDTO.Team.Id.ToString(), mappedStatistic.Team);
            Assert.Equal(statisticImportDTO.League.Id.ToString(), mappedStatistic.League);
        }
    }
}
