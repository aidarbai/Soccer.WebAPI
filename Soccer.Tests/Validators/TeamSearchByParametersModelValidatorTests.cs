using Soccer.BLL.Validators;
using Soccer.COMMON.ViewModels;

namespace Soccer.Tests.Validators
{
    public class TeamSearchByParametersModelValidatorTests
    {
        public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { new TeamSearchModel { LeagueId = "140" }, true },
            new object[] { new TeamSearchModel { LeagueId = "123412341234" }, false },
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void Validator_ValidatesCorrectly(TeamSearchModel searchModel, bool expectedValid)
        {
            //Arrange
            var sut = new TeamSearchByParametersModelValidator();

            //Act
            var actualValid = sut.Validate(searchModel).IsValid;

            //Assert
            Assert.Equal(expectedValid, actualValid);
        }
    }
}
