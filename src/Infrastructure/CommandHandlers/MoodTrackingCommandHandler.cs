using RS.MF.MoodTracking.Infrastructure.Validators;

namespace RS.MF.MoodTracking.Infrastructure.CommandHandlers
{
    public class MoodTrackingCommandHandler : ICommandHandler<MoodTrackingCommand, RequestResponse<CommandResult>>
    {
        private readonly IRepository<MoodTrackingAggregate> _repository;
        private readonly MoodTrackingCommandValidator validations;

        public MoodTrackingCommandHandler(
            IRepository<MoodTrackingAggregate> repository,
            MoodTrackingCommandValidator validations
            )
        {
            _repository = repository;
            this.validations = validations;
        }

        public async Task<RequestResponse<CommandResult>> Handle(MoodTrackingCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validations.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }
            var @event = Mapping.Map<MoodTrackingCommand, MoodTrackingEvent>(request);
            @event.CorrelationId = request.CorrelationId;
            var data = MoodTrackingAggregate.Create(@event);
            await _repository.Add(data);

            return new RequestResponse<CommandResult>
            {
                Payload = new CommandResult
                {
                    Id = data.Id
                }
            };
        }
    }
}