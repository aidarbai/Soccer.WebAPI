using AutoFixture;
using AutoFixture.Xunit2;
using Soccer.COMMON.ViewModels;

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
}