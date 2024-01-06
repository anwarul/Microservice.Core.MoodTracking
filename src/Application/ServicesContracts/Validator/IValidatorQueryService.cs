
namespace RS.MF.MoodTracking.Application.ServicesContracts.Validator
{
    public interface IValidatorQueryService
    {
        Task<bool> CheckValidActivity(Guid itemId, CancellationToken cancellationToken);
        Task<bool> CheckValidActivityGroup(Guid itemId, CancellationToken cancellationToken);
        Task<bool> IsExistingTrackingLog(Guid itemId, CancellationToken cancellationToken);
    }
}
