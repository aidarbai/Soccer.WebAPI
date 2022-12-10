using FluentValidation;
using Soccer.COMMON.Constants;
using Soccer.COMMON.ViewModels;
using System.Linq;

namespace Soccer.BLL.Validators
{
    public class PlayerSearchByParametersModelValidator : AbstractValidator<PlayerSearchByParametersModel>
    {
        public PlayerSearchByParametersModelValidator()
        {
            When(x => x.DateOfBirthFrom.HasValue || x.DateOfBirthTo.HasValue, () =>
            {
                RuleFor(x => x.AgeFrom)
                    .Equal(0)
                        .WithMessage("Please pick either age or date of birth for searching players");
                RuleFor(x => x.AgeTo)
                    .Equal(0)
                        .WithMessage("Please pick either age or date of birth for searching players");
            });

            When(x => x.AgeFrom > 0 || x.AgeTo > 0, () =>
            {
                RuleFor(x => x.DateOfBirthFrom)
                    .Null()
                        .WithMessage("Please pick either age or date of birth for searching players");
                RuleFor(x => x.DateOfBirthTo)
                    .Null()
                        .WithMessage("Please pick either age or date of birth for searching players");
            });

            RuleFor(x => x.AgeFrom)
                .GreaterThanOrEqualTo(0).WithMessage("Age can't be a negative number");

            RuleFor(x => x.AgeTo)
                .GreaterThanOrEqualTo(0).WithMessage("Age can't be a negative number");

            RuleFor(p => p.SortBy)
                .IsInEnum();

            RuleFor(p => p.Order)
                .IsInEnum();

            RuleFor(p => p.PageNumber)
                .GreaterThanOrEqualTo(0);

            //RuleFor(p => p.PageSize)
            //    .IsInEnum();

            RuleFor(p => p.PageSize)
                .Must(x => AppConstants.pageSize.Contains(x))
                .WithMessage("PageSize must be 10, 25, 50 or 100");
        }
    }
}
