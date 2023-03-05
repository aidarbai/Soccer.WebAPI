using AutoFixture;
using Soccer.BLL.Validators;
using Soccer.COMMON.ViewModels;
using System.Reflection;

namespace Soccer.Tests.Customizations
{
    public class PlayerSearchModelData : AutoDataAttribute
    {
        public PlayerSearchModelData() : base(() =>
        {
            var fixture = new Fixture();

            fixture.Customize<PlayerSearchByParametersModel>(composer =>
                composer.With(p => p.Name, "Ben")
                        .With(p => p.AgeFrom, 18)
                        .With(p => p.AgeTo, 22)
                        .With(p => p.PageNumber, 0)
                        .With(p => p.PageSize, 10)
                        .Without(p => p.CardsFrom)
                        .Without(p => p.CardsTo)
                        .Without(p => p.DateOfBirthFrom)
                        .Without(p => p.DateOfBirthTo));

            return fixture;
        })
        { }
    }

    //public static IEnumerable<object[]> Data =>
    //    new List<object[]>
    //    {
    //        new object[] { new PlayerSearchByParametersModel { TeamId = "1" }, true },
    //        new object[] { new PlayerSearchByParametersModel { TeamId = "1234567891011" }, false }
    //    };

    //[Theory]
    //[AutoDomainData(nameof(Data))]
    //public void Validator_ValidateCorrectly(CountQuote.Request request, bool expectedValid)
    //{
    //    //arrange
    //    //var sut = new CountQuote.Validator();
    //    var sut = new PlayerSearchByParametersModelValidator();
        

    //    //act
    //    var actualValid = sut.Validate(request).IsValid;

    //    //assert
    //    actualValid.Should().Be(expectedValid);
    //}
}