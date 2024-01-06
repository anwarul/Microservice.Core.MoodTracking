using RS.MF.MoodTracking.Application.ServicesContracts.Validator;

namespace RS.MF.MoodTracking.Infrastructure.Services.Validation
{
    public class ValidatorQueryService : IValidatorQueryService
    {

        private readonly ISAASMongoRepository _repository;
        private readonly DefaultValueInjection _defaultValueInjection;
        private readonly SecurityContext _securityContext;

        public ValidatorQueryService(
            ISAASMongoRepository repository, DefaultValueInjection defaultValueInjection, ISecurityContextProvider securityContext)
        {
            _repository = repository;
            _securityContext = securityContext.GetSecurityContext();
            _defaultValueInjection = defaultValueInjection;
        }

        public async Task<bool> CheckValidActivity(Guid itemId, CancellationToken cancellationToken)
        {
            return await _repository.ExistsAsync<MoodActivity>(_securityContext.TenantId, obj => itemId.Equals(obj.ItemId), cancellationToken);
        }

        public async Task<bool> CheckValidActivityGroup(Guid itemId, CancellationToken cancellationToken)
        {
            return await _repository.ExistsAsync<MoodActivityGroup>(_securityContext.TenantId, obj => itemId.Equals(obj.ItemId), cancellationToken);
        }

        public async Task<bool> IsExistingTrackingLog(Guid itemId, CancellationToken cancellationToken)
        {
            var data = await _repository.GetItemAsync<MoodTrackingLog>(_securityContext.TenantId, obj => itemId.Equals(obj.ItemId), cancellationToken);
            return data != null;
        }
    }
}
