using RS.MF.MoodTracking.Application.ServicesContracts.Validator;

namespace RS.MF.MoodTracking.Infrastructure.Validators
{
    public class MoodTrackingEditCommandValidator : AbstractValidator<MoodTrackingEditCommand>
    {
        private readonly IValidatorQueryService _queryValidation;

        public MoodTrackingEditCommandValidator(IValidatorQueryService queryValidation)
        {
            _queryValidation = queryValidation;
            RuleFor(cmd => cmd.ItemId)
                 .Cascade(CascadeMode.StopOnFirstFailure)
                 .NotEmpty()
                 .WithMessage("Mood Tracking id is required !")
                 .NotNull()
                 .WithMessage("Mood Tracking id is required !")
                 .MustAsync(_queryValidation.IsExistingTrackingLog)
                 .WithMessage("This Mood Tracking Id doesn't exist ! Please set valid Id.");
        }
    }
}