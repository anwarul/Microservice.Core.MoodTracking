namespace RS.MF.MoodTracking.Infrastructure.EventHandlers
{
    public class MoodTrackingEventHandler : EventConsumerHandler<MoodTrackingEvent>
    {
        private readonly IMoodTrackingService _moodTracking;

        public MoodTrackingEventHandler(IMoodTrackingService moodTracking)
        {
            _moodTracking = moodTracking;
        }

        public override async Task Handle(MoodTrackingEvent @event, CancellationToken cancellationToken)
        {
            await _moodTracking.Processor(@event, cancellationToken);
        }
    }
}