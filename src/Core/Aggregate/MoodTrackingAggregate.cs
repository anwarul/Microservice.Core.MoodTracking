namespace RS.MF.MoodTracking.Core.Aggregate
{
    public class MoodTrackingAggregate : RedlimeSolutions.Microservice.Framework.Core.EventStores.Aggregate.Aggregate
    {
        public string Description { get; private set; }
        public DateTimeOffset CreateDate { get; private set; }

        public MoodTrackingAggregate(MoodTrackingEvent @event)
        {
            this.Enqueue(@event);
            this.Apply(@event);
        }

        public MoodTrackingAggregate(MoodTrackingEditEvent @event)
        {
            this.Enqueue(@event);
            this.Apply(@event);
        }

        public static MoodTrackingAggregate Create(MoodTrackingEvent @event)
        {
            return new MoodTrackingAggregate(@event);
        }

        public static MoodTrackingAggregate Edit(MoodTrackingEditEvent @event)
        {
            return new MoodTrackingAggregate(@event);
        }

        private void Apply(MoodTrackingEvent @event)
        {
            Id = @event.EventItemId;
            Description = typeof(MoodTrackingEvent).Name;
            CreateDate = DateTimeOffset.Now;
        }

        private void Apply(MoodTrackingEditEvent @event)
        {
            Id = @event.EventItemId;
            Description = typeof(MoodTrackingEditEvent).Name;
            CreateDate = DateTimeOffset.Now;
        }
    }
}