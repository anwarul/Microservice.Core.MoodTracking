namespace RS.MF.MoodTracking.Infrastructure.DbEngineRepositories
{
    public class MoodEntryRepository : IMoodEntryRepository
    {
        private readonly ISAASMongoRepository _repository;
        private readonly DefaultValueInjection _defaultValueInjection;

        public MoodEntryRepository(
            ISAASMongoRepository repository, DefaultValueInjection defaultValueInjection)
        {
            _repository = repository;
            _defaultValueInjection = defaultValueInjection;
        }

        public async Task AddAsync(string tenantId, MoodEntry entity, CancellationToken cancellationToken)
        {
            if (entity.ItemId == Guid.Empty)
                entity.ItemId = Guid.NewGuid();
            _defaultValueInjection.Inject<MoodEntry>(entity);
            await _repository.SaveAsync<MoodEntry>(tenantId, entity, cancellationToken);
        }

        public async Task DeleteAsync(string tenantId, Guid itemId, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync<MoodEntry>(tenantId, obj => itemId.Equals(obj.ItemId));
        }

        public ValueTask<MoodEntry> GetByIdAsync(string tenantId, Guid itemId, CancellationToken cancellationToken)
        {
            return _repository.GetItemAsync<MoodEntry>(tenantId, obj => itemId.Equals(obj.ItemId));
        }

        public async Task<IQueryable<MoodEntry>> Query(string tenantId)
        {
            var query = await _repository.GetQueryabl<MoodEntry, MoodEntry>(tenantId, obj => obj.IsMarkedToDelete == false, obj => obj);

            return query;
        }

        public async Task UpdateAsync(string tenantId, JsonPatchDocument<MoodEntry> entity, Guid itemId, CancellationToken cancellationToken)
        {
            try
            {
                var exMoodActivity = await GetByIdAsync(tenantId, itemId, cancellationToken);
                if (exMoodActivity != null)
                {
                    entity.ApplyTo(exMoodActivity);
                    _defaultValueInjection.Inject<MoodEntry>(exMoodActivity);
                    await _repository.UpdateAsync<MoodEntry>(tenantId, obj => obj.ItemId == itemId, exMoodActivity);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}