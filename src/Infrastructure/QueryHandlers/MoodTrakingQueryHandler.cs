using RedlimeSolutions.Microservice.Framework.Core.Queries;
using RS.MF.MoodTracking.Application.Dtos;
using RS.MF.MoodTracking.Application.Queries;

namespace RS.MF.MoodTracking.Infrastructure.QueryHandlers
{
    public class MoodTrakingQueryHandler : IQueryHandler<UserMoodQuery, RequestResponse<IEnumerable<MoodTrackingView>>>
    {
        private readonly IMoodTrackingQueryService _queryService;

        public MoodTrakingQueryHandler(
            IMoodTrackingQueryService queryService
            )
        {
            _queryService = queryService;
        }


        public async Task<RequestResponse<IEnumerable<MoodTrackingView>>> Handle(UserMoodQuery request, CancellationToken cancellationToken)
        {
            var result = await _queryService.GetMoodTrackings(request, cancellationToken);
            return new RequestResponse<IEnumerable<MoodTrackingView>>
            {
                Payload = result
            };
        }
    }
}
