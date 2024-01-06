using RSMF.Entity.Security;
using System.Linq.Expressions;
using System.Threading;

namespace RS.MF.MoodTracking.Core.Contracts
{
    public interface IMoodTrackingRepository
    {
        Task AddAsync(string tenantId, MoodTrackingLog entity, CancellationToken cancellationToken);

        Task UpdateAsync(string tenantId, JsonPatchDocument<MoodTrackingLog> entity, Guid itemId, UserBasicInfo lastUpdateUser, CancellationToken cancellationToken);

        Task DeleteAsync(string tenantId, Guid itemId, CancellationToken cancellationToken);
        Task<IQueryable<MoodTrackingLog>> Query(string tenantId);
        Task<IMongoQueryable<Y>> Query<Y>(string tenantId, Expression<Func<MoodTrackingLog, bool>> dataFilters, Expression<Func<MoodTrackingLog, Y>> projection);
        ValueTask<MoodTrackingLog> GetByIdAsync(string tenantId, Guid itemId, CancellationToken cancellationToken);
    }
}