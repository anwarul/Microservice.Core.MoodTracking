namespace RS.MF.MoodTracking.Core.Aggregate
{
    public class MoodAdministrativeAggregate : RedlimeSolutions.Microservice.Framework.Core.EventStores.Aggregate.Aggregate
    {
        public string Description { get; private set; }
        public DateTimeOffset CreateDate { get; private set; }

        public MoodAdministrativeAggregate(MoodEntryEvent @event)
        {
            this.Enqueue(@event);
            this.Apply(@event);
        }
        public MoodAdministrativeAggregate(MoodEntryEditEvent @event)
        {
            this.Enqueue(@event);
            this.Apply(@event);
        }
        public MoodAdministrativeAggregate(MoodActivityEditEvent @event)
        {
            this.Enqueue(@event);
            this.Apply(@event);
        }
        public MoodAdministrativeAggregate(MoodActivityEvent @event)
        {
            this.Enqueue(@event);
            this.Apply(@event);
        }

        public MoodAdministrativeAggregate Create(MoodEntryEvent @event)
        {
            return new MoodAdministrativeAggregate(@event);
        }

        public MoodAdministrativeAggregate Edit(MoodEntryEditEvent @event)
        {
            return new MoodAdministrativeAggregate(@event);
        }
        public MoodAdministrativeAggregate Create(MoodActivityEvent @event)
        {
            return new MoodAdministrativeAggregate(@event);
        }
        public MoodAdministrativeAggregate Edit(MoodActivityEditEvent @event)
        {
            return new MoodAdministrativeAggregate(@event);
        }

        private void Apply(MoodEntryEvent @event)
        {
            Id = @event.EventItemId;
            Description = typeof(MoodEntryEvent).Name;
            CreateDate = DateTimeOffset.Now;
        }
        private void Apply(MoodEntryEditEvent @event)
        {
            Id = @event.EventItemId;
            Description = typeof(MoodEntryEditEvent).Name;
            CreateDate = DateTimeOffset.Now;
        }
        private void Apply(MoodActivityEditEvent @event)
        {
            Id = @event.EventItemId;
            Description = typeof(MoodActivityEditEvent).Name;
            CreateDate = DateTimeOffset.Now;
        }
        private void Apply(MoodActivityEvent @event)
        {
            Id = @event.EventItemId;
            Description = typeof(MoodActivityEvent).Name;
            CreateDate = DateTimeOffset.Now;
        }
    }
}