namespace RS.MF.MoodTracking.Core.Events
{
    public class MoodActivityEvent : MoodActivityDto, IEventExtension
    {
        public NotifySubscribeData NotifySubscribeData { get; set; }
        public Guid CreatedByUserId { get; set; }
        public string Language { get; set; }
        public Guid TenantId { get; set; }
        public Guid LastUpdatedByUserId { get; set; }
        public Guid SiteId { get; set; }
        public Guid EventItemId { get; set; } = Guid.NewGuid();
        public string HostDomain { get; set; }
        public string CorrelationId { get; set; }
    }
}