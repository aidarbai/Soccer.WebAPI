using Soccer.BLL.Validators;
using Soccer.COMMON.ViewModels;

namespace Soccer.Tests.Validators
{
    public class PlayerSearchByParametersModelValidatorTests
    {
        public static IEnumerable<object[]> Data =>
        new List<object[]>
        {
            new object[] { new PlayerSearchByParametersModel { TeamId = "530" }, true },
            new object[] { new PlayerSearchByParametersModel { TeamId = "123412341234" }, false },
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void Validator_ValidatesCorrectly(PlayerSearchByParametersModel searchModel, bool expectedValid)
        {
            //Arrange
            var sut = new PlayerSearchByParametersModelValidator();

            //Act
            var actualValid = sut.Validate(searchModel).IsValid;

            //Assert
            Assert.Equal(expectedValid, actualValid);
        }
    }
}
