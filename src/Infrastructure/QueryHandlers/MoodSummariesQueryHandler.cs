using RedlimeSolutions.Microservice.Framework.Core.Queries;
using RS.MF.MoodTracking.Application.Dtos;
using RS.MF.MoodTracking.Application.Queries;

namespace RS.MF.MoodTracking.Infrastructure.QueryHandlers
{
    public class MoodSummariesQueryHandler : IQueryHandler<UserMoodSummaryQuery, RequestResponse<IEnumerable<MoodSummary>>>
    {
        private readonly IMoodTrackingQueryService _queryService;

        public MoodSummariesQueryHandler(
            IMoodTrackingQueryService queryService
            )
        {
            _queryService = queryService;
        }


        public async Task<RequestResponse<IEnumerable<MoodSummary>>> Handle(UserMoodSummaryQuery request, CancellationToken cancellationToken)
        {
            var result = await _queryService.GetMoodSummaries(request, cancellationToken);
            return new RequestResponse<IEnumerable<MoodSummary>>
            {
                Payload = result
            };
        }
    }
}
