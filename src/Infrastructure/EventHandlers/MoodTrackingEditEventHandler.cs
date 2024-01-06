namespace RS.MF.MoodTracking.Infrastructure.EventHandlers
{
    public class MoodTrackingEditEventHandler : EventConsumerHandler<MoodTrackingEditEvent>
    {
        private readonly IMoodTrackingService _moodTracking;

        public MoodTrackingEditEventHandler(IMoodTrackingService moodTracking)
        {
            _moodTracking = moodTracking;
        }

        public override async Task Handle(MoodTrackingEditEvent @event, CancellationToken cancellationToken)
        {
            await _moodTracking.Processor(@event, cancellationToken);
        }
    }
}