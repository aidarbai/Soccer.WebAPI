using Soccer.BLL.Validators;
using Soccer.COMMON.ViewModels;

namespace Soccer.Tests.Validators
{
    public class LeagueSearchByParametersModelValidatorTests
    {
        public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { new LeagueSearchModel { PageNumber = 1 }, true },
            new object[] { new LeagueSearchModel { PageNumber = -25 }, false },
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void Validator_ValidatesCorrectly(LeagueSearchModel searchModel, bool expectedValid)
        {
            //Arrange
            var sut = new LeagueSearchByParametersModelValidator();

            //Act
            var actualValid = sut.Validate(searchModel).IsValid;

            //Assert
            Assert.Equal(expectedValid, actualValid);
        }
    }
}
