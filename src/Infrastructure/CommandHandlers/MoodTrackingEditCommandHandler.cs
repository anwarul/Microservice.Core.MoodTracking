using RS.MF.MoodTracking.Infrastructure.Validators;

namespace RS.MF.MoodTracking.Infrastructure.CommandHandlers
{
    public class MoodTrackingEditCommandHandler : ICommandHandler<MoodTrackingEditCommand, RequestResponse<CommandResult>>
    {
        private readonly IRepository<MoodTrackingAggregate> _repository;
        private readonly MoodTrackingEditCommandValidator validations;
        public MoodTrackingEditCommandHandler(
            IRepository<MoodTrackingAggregate> repository,
            MoodTrackingEditCommandValidator validations
            )
        {
            _repository = repository;
            this.validations = validations;
        }

        public async Task<RequestResponse<CommandResult>> Handle(MoodTrackingEditCommand request, CancellationToken cancellationToken)
        {
            var validationResult = await validations.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }
            var @event = new MoodTrackingEditEvent()
            {
                MoodTrackingId = request.ItemId,
                Document = JsonConvert.SerializeObject(request.Document),
                CorrelationId = request.CorrelationId,
            };
            var data = MoodTrackingAggregate.Edit(@event);
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