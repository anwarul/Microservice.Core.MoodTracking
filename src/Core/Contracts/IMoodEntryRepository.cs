using System.Threading;

namespace RS.MF.MoodTracking.Core.Contracts
{
    public interface IMoodEntryRepository
    {
        Task AddAsync(string tenantId, MoodEntry entity, CancellationToken cancellationToken);

        Task UpdateAsync(string tenantId, JsonPatchDocument<MoodEntry> entity, Guid itemId, CancellationToken cancellationToken);

        Task DeleteAsync(string tenantId, Guid itemId, CancellationToken cancellationToken);

        Task<IQueryable<MoodEntry>> Query(string tenantId);

        ValueTask<MoodEntry> GetByIdAsync(string tenantId, Guid itemId, CancellationToken cancellationToken);
    }
}