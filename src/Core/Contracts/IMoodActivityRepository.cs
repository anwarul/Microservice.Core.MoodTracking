using System.Threading;

namespace RS.MF.MoodTracking.Core.Contracts
{
    public interface IMoodActivityRepository
    {
        Task AddAsync(string tenantId, MoodActivity entity, CancellationToken cancellationToken);

        Task UpdateAsync(string tenantId, JsonPatchDocument<MoodActivity> entity, Guid itemId, CancellationToken cancellationToken);

        Task DeleteAsync(string tenantId, Guid itemId, CancellationToken cancellationToken);

        Task<IQueryable<MoodActivity>> Query(string tenantId);

        ValueTask<MoodActivity> GetByIdAsync(string tenantId, Guid itemId, CancellationToken cancellationToken);
    }
}