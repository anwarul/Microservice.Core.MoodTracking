namespace RS.MF.MoodTracking.Infrastructure.DbEngineRepositories
{
    public class MoodActivityRepository : IMoodActivityRepository
    {
        private readonly ISAASMongoRepository _repository;
        private readonly DefaultValueInjection _defaultValueInjection;

        public MoodActivityRepository(
            ISAASMongoRepository repository, DefaultValueInjection defaultValueInjection)
        {
            _repository = repository;
            _defaultValueInjection = defaultValueInjection;
        }

        public async Task AddAsync(string tenantId, MoodActivity entity, CancellationToken cancellationToken)
        {
            if (entity.ItemId == Guid.Empty)
                entity.ItemId = Guid.NewGuid();
            _defaultValueInjection.Inject<MoodActivity>(entity);
            await _repository.SaveAsync<MoodActivity>(tenantId, entity, cancellationToken);
        }

        public async Task DeleteAsync(string tenantId, Guid itemId, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync<MoodActivity>(tenantId, obj => itemId.Equals(obj.ItemId));
        }

        public ValueTask<MoodActivity> GetByIdAsync(string tenantId, Guid itemId, CancellationToken cancellationToken)
        {
            return _repository.GetItemAsync<MoodActivity>(tenantId, obj => itemId.Equals(obj.ItemId));
        }

        public async Task<IQueryable<MoodActivity>> Query(string tenantId)
        {
            var query = await _repository.GetQueryabl<MoodActivity, MoodActivity>(tenantId, obj => obj.IsMarkedToDelete == false, obj => obj);

            return query;
        }

        public async Task UpdateAsync(string tenantId, JsonPatchDocument<MoodActivity> entity, Guid itemId, CancellationToken cancellationToken)
        {
            try
            {
                var exMoodActivity = await GetByIdAsync(tenantId, itemId, cancellationToken);
                if (exMoodActivity != null)
                {
                    entity.ApplyTo(exMoodActivity);
                    _defaultValueInjection.Inject<MoodActivity>(exMoodActivity);
                    await _repository.UpdateAsync<MoodActivity>(tenantId, obj => obj.ItemId == itemId, exMoodActivity);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}