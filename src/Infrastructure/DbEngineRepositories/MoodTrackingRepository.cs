using RSMF.Entity.Security;
using System.Linq.Expressions;

namespace RS.MF.MoodTracking.Infrastructure.DbEngineRepositories
{
    public class MoodTrackingRepository : IMoodTrackingRepository
    {
        private readonly ISAASMongoRepository _repository;
        private readonly DefaultValueInjection _defaultValueInjection;

        public MoodTrackingRepository(
            ISAASMongoRepository repository, DefaultValueInjection defaultValueInjection)
        {
            _repository = repository;
            _defaultValueInjection = defaultValueInjection;
        }

        public async Task AddAsync(string tenantId, MoodTrackingLog entity, CancellationToken cancellationToken)
        {
            if (entity.ItemId == Guid.Empty)
                entity.ItemId = Guid.NewGuid();
            _defaultValueInjection.Inject<MoodTrackingLog>(entity);
            await _repository.SaveAsync<MoodTrackingLog>(tenantId, entity, cancellationToken);
        }

        public async Task DeleteAsync(string tenantId, Guid itemId, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync<MoodTrackingLog>(tenantId, obj => itemId.Equals(obj.ItemId));
        }

        public ValueTask<MoodTrackingLog> GetByIdAsync(string tenantId, Guid itemId, CancellationToken cancellationToken)
        {
            return _repository.GetItemAsync<MoodTrackingLog>(tenantId, obj => itemId.Equals(obj.ItemId));
        }

        public async Task<IQueryable<MoodTrackingLog>> Query(string tenantId)
        {
            var query = await _repository.GetQueryabl<MoodTrackingLog, MoodTrackingLog>(tenantId, obj => obj.IsMarkedToDelete == false, obj => obj);

            return query;
        }
        public async Task<IMongoQueryable<Y>> Query<Y>(string tenantId, Expression<Func<MoodTrackingLog, bool>> dataFilters, Expression<Func<MoodTrackingLog, Y>> projection)
        {
            var query = await _repository.GetQueryabl<MoodTrackingLog, Y>(tenantId, dataFilters, projection: projection);
            return query;
        }

        public async Task UpdateAsync(string tenantId, JsonPatchDocument<MoodTrackingLog> entity, Guid itemId, UserBasicInfo lastUpdateUser, CancellationToken cancellationToken)
        {
            try
            {
                var exMoodActivity = await GetByIdAsync(tenantId, itemId, cancellationToken);
                if (exMoodActivity != null)
                {
                    entity.ApplyTo(exMoodActivity);
                    exMoodActivity.LastUpdatedByUser = lastUpdateUser;
                    exMoodActivity.LastUpdatedByUserId = lastUpdateUser.ItemId;
                    _defaultValueInjection.Inject<MoodTrackingLog>(exMoodActivity);
                    await _repository.UpdateAsync<MoodTrackingLog>(tenantId, obj => obj.ItemId == itemId, exMoodActivity);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}