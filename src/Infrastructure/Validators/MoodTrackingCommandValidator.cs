using RS.MF.MoodTracking.Application.ServicesContracts.Validator;

namespace RS.MF.MoodTracking.Infrastructure.Validators
{
    public class MoodTrackingCommandValidator : AbstractValidator<MoodTrackingCommand>
    {
        private readonly IValidatorQueryService _queryValidation;
        public MoodTrackingCommandValidator(IValidatorQueryService queryValidation)
        {
            _queryValidation = queryValidation;


            RuleFor(cmd => cmd.Mood)
              .Cascade(CascadeMode.StopOnFirstFailure)
              .NotEmpty()
              .WithMessage("Mood info can't be empty")
              .NotNull()
              .WithMessage("Mood info can't be empty");


            RuleFor(cmd => cmd.Mood.ReferenceItemId)
              .Cascade(CascadeMode.StopOnFirstFailure)
              .NotEmpty()
              .WithMessage("Mood id can't be empty")
              .NotNull()
              .NotEqual(Guid.Empty)
              .WithMessage("Have to provide valid mood id.");


            RuleFor(cmd => cmd.Mood)
              .Cascade(CascadeMode.StopOnFirstFailure)
              .NotEmpty()
              .WithMessage("Mood activity info can't be empty")
              .NotNull()
              .WithMessage("Mood activity info can't be empty");


            RuleForEach(cmd => cmd.Activity).SetValidator(new MoodActivityValidator(_queryValidation));
        }
    }

    public class MoodActivityValidator : AbstractValidator<MoodActivityReference>
    {
        private readonly IValidatorQueryService _queryValidation;
        public MoodActivityValidator(IValidatorQueryService queryValidation)
        {
            _queryValidation = queryValidation;


            RuleFor(cmd => cmd.ReferenceItemId)
              .Cascade(CascadeMode.StopOnFirstFailure)
              .NotEmpty()
              .WithMessage("Mood activity id can't be empty")
              .NotNull()
              .NotEqual(Guid.Empty)
              .WithMessage("Have to provide valid mood activity id.");
            //.MustAsync(_queryValidation.CheckValidActivity)
            //.WithMessage("This Activity Id doesn't exist ! Please set valid Id.");

            RuleFor(cmd => cmd.Name)
              .Cascade(CascadeMode.StopOnFirstFailure)
              .NotEmpty()
              .WithMessage("Mood activity name can't be empty")
              .NotNull()
              .WithMessage("Have to provide valid mood activity name.");
        }
    }

    public class ActivityGroupValidator : AbstractValidator<MoodActivityGroupReference>
    {
        private readonly IValidatorQueryService _queryValidation;
        public ActivityGroupValidator(IValidatorQueryService queryValidation)
        {
            _queryValidation = queryValidation;


            RuleFor(cmd => cmd.ReferenceItemId)
              .Cascade(CascadeMode.StopOnFirstFailure)
              .NotEmpty()
              .WithMessage("ReferenceItemId can't be empty")
              .NotNull()
              .NotEqual(Guid.Empty)
              .WithMessage("Have to provide valid mood activity id.")
              .MustAsync(_queryValidation.CheckValidActivityGroup)
              .WithMessage("This Activity Group Id doesn't exist ! Please set valid Id.");

            RuleFor(cmd => cmd.Name)
              .Cascade(CascadeMode.StopOnFirstFailure)
              .NotEmpty()
              .WithMessage("Activity Group can't be empty")
              .NotNull()
              .WithMessage("Have to provide valid Activity Group.")
              ;
        }
    }
}