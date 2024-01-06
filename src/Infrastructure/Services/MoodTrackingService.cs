namespace RS.MF.MoodTracking.Infrastructure.Services
{
    public class MoodTrackingService : ContextUser, IMoodTrackingService
    {
        private readonly IMoodTrackingRepository _repository;
        private readonly IDistributCache _distributCache;
        private readonly ISecurityContextProvider _securityContextProvider;

        public MoodTrackingService(
            IMoodTrackingRepository repository,
            IDistributCache distributCache,
            ISecurityContextProvider securityContextProvider
            ) : base(securityContextProvider)
        {
            _repository = repository;
            _distributCache = distributCache;
            _securityContextProvider = securityContextProvider;
        }

        public async Task Processor(MoodTrackingEvent @event, CancellationToken cancellationToken)
        {
            var token = _distributCache.GetValue(@event.EventItemId.ToString());
            var user = await this.GetUser(token);
            MoodTrackingLog obj = new MoodTrackingLog();
            obj.ItemId = @event.EventItemId;
            obj.TenantId = @event.TenantId;
            obj.SiteId = @event.SiteId;
            obj.CorrelationId = @event.CorrelationId;
            obj.CreatedByUserId = @event.CreatedByUserId;
            obj.CreatedByUser = user;
            obj.Note = @event.Note;
            obj.ActionDateTime = @event.ActionDateTime;
            obj.Activity = @event.Activity?.Select(_obj =>
            {
                var activity = new MoodActivityReference
                {
                    Name = _obj.Name,
                    ReferenceItemId = _obj.ReferenceItemId,
                    ImageUrl = _obj.ImageUrl
                };
                return activity;
            })?.ToList();
            obj.Mood = new MoodEntryReference
            {
                ImageUrl = @event.Mood.ImageUrl,
                ReferenceItemId = @event.Mood.ReferenceItemId,
                Name = @event.Mood.Name,
            };
            await _repository.AddAsync(@event.TenantId.ToString(), obj, cancellationToken);
        }

        public async Task Processor(MoodTrackingEditEvent @event, CancellationToken cancellationToken)
        {
            var token = _distributCache.GetValue(@event.EventItemId.ToString());
            var user = await this.GetUser(token);
            JsonPatchDocument<MoodTrackingLog> entity = JsonConvert.DeserializeObject<JsonPatchDocument<MoodTrackingLog>>(@event.Document);

            await _repository.UpdateAsync(@event.TenantId.ToString(), entity, @event.MoodTrackingId, user, cancellationToken);
        }
    }
}