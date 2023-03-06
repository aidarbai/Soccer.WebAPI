using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Soccer.Tests.Services
{
    public class HttpClientServiceTests
    {
        private readonly IConfigurationRoot configuration;
        private readonly HttpClient httpClient;
        private readonly Mock<ILogger<HttpClientService>> logger;

        private readonly HttpClientService sut;

        public HttpClientServiceTests()
        {
            string appSettings = @"{""Football-API"":{
                                                         ""LeagueId"" : ""140"",
                                                         ""LeagueById"" : ""https://api-football-v1.p.rapidapi.com/v3/leagues?id={0}""
                                                         }
                                       }";

            var builder = new ConfigurationBuilder();
            builder.AddJsonStream(new MemoryStream(Encoding.UTF8.GetBytes(appSettings)));
            configuration = builder.Build();

            var mockedHandler = new Mock<HttpMessageHandler>();
            httpClient = new HttpClient(mockedHandler.Object);

            logger = new();

            sut = new(httpClient, configuration, logger.Object);
        }

        [Fact(DisplayName = "Check If Exception Is Handled Correctly")]
        public async Task Test1_HttpClientServiceAsync()
        {
            //Act
            var test = await sut.GetDataAsync<string>("url");

            //Assert
            logger.Verify(x => x.Log(It.Is<LogLevel>(l => l == LogLevel.Error),
                                                                    It.IsAny<EventId>(),
                                                                     It.Is<It.IsAnyType>((v, t) => v.ToString() == "An invalid request URI was provided. Either the request URI must be an absolute URI or BaseAddress must be set."),
                                                                  null,
                                                                  It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)), Times.Once);
            Assert.Null(test);
        }
    }
}
