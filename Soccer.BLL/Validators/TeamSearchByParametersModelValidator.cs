using FluentValidation;
using Soccer.COMMON.Constants;
using Soccer.COMMON.ViewModels;

namespace Soccer.BLL.Validators
{
    public class TeamSearchByParametersModelValidator : AbstractValidator<TeamSearchModel>
    {
        public TeamSearchByParametersModelValidator()
        {
            RuleFor(x => x.LeagueId)
                .MaximumLength(10);

            RuleFor(x => x.Name)
                .MaximumLength(50);

            
            RuleFor(p => p.SortBy)
                .IsInEnum();

            RuleFor(p => p.Order)
                .IsInEnum();

            RuleFor(p => p.PageNumber)
                .GreaterThanOrEqualTo(0);

            RuleFor(p => p.PageSize)
                .Must(x => AppConstants.pageSize.Contains(x))
                .WithMessage("PageSize must be 10, 25, 50 or 100");
        }
    }
}
