using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System.Text;

namespace Soccer.Tests.Services
{
    public class ImportServiceTests
    {
        private readonly IConfigurationRoot configuration;
        private readonly IMapper mapper;
        private readonly Mock<ILeagueService> leagueService;
        private readonly Mock<ITeamService> teamService;
        private readonly Mock<IPlayerService> playerService;
        private readonly Mock<IHttpClientService> dataDownloader;
        private readonly Mock<ILogger<ImportService>> logger;

        private readonly ImportService sut;

        public ImportServiceTests()
        {
            string appSettings = @"{""Football-API"":{
                                                         ""LeagueId"" : ""140"",
                                                         ""LeagueById"" : ""https://api-football-v1.p.rapidapi.com/v3/leagues?id={0}""
                                                         }
                                       }";

            var builder = new ConfigurationBuilder();

            builder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(appSettings)));

            configuration = builder.Build();

            mapper = new MapperConfiguration(x => x.AddProfile(new LeagueMap())).CreateMapper();
            leagueService = new();
            teamService = new();
            playerService = new();
            dataDownloader = new();
            logger = new();


            sut = new(dataDownloader.Object,
                      leagueService.Object,
                      configuration,
                      teamService.Object,
                      playerService.Object,
                      mapper,
                      logger.Object);
        }

        [Fact(DisplayName = "ImportLeagueAsync Should Not Start Processing Result From DataDownloader If Its Null")]

        public async Task Test1_ImportLeagueAsync()
        {
            //Arange
            ResponseImportDTO<ResponseLeagueImportDTO>? model = null;
            dataDownloader.Setup(x => x.GetDataAsync<ResponseImportDTO<ResponseLeagueImportDTO>>(It.IsAny<string>())).ReturnsAsync(model);

            //Act
            await sut.ImportLeagueAsync();

            //Assert
            dataDownloader.Verify(d => d.GetDataAsync<ResponseImportDTO<ResponseLeagueImportDTO>>(It.IsAny<string>()), Times.Once);
            leagueService.Verify(m => m.CreateAsync(It.IsAny<League>()), Times.Never);
        }

        [Theory(DisplayName = "ImportLeagueAsync Should Process Result From DataDownloader If Its Not Null")]
        [AutoData]
        public async Task Test2_ImportLeagueAsync(ResponseImportDTO<ResponseLeagueImportDTO> responseImport)
        {
            //Arange
            dataDownloader.Setup(x => x.GetDataAsync<ResponseImportDTO<ResponseLeagueImportDTO>>(It.IsAny<string>())).ReturnsAsync(responseImport);

            //Act
            await sut.ImportLeagueAsync();

            //Assert
            dataDownloader.Verify(d => d.GetDataAsync<ResponseImportDTO<ResponseLeagueImportDTO>>(It.IsAny<string>()), Times.Once);
            leagueService.Verify(m => m.CreateAsync(It.IsAny<League>()), Times.Once);
        }
    }
}