using RedlimeSolutions.Microservice.Framework.Core.Queries;
using RS.MF.MoodTracking.Application.Dtos;
using RS.MF.MoodTracking.Application.Queries;

namespace RS.MF.MoodTracking.Infrastructure.QueryHandlers
{
    public class MoodAndActivitySummaryQueryHandler : IQueryHandler<MoodAndActivitySummariesQuery, RequestResponse<MoodAndActivitySummariesView>>
    {
        private readonly IMoodTrackingQueryService _queryService;

        public MoodAndActivitySummaryQueryHandler(
            IMoodTrackingQueryService queryService
            )
        {
            _queryService = queryService;
        }


        public async Task<RequestResponse<MoodAndActivitySummariesView>> Handle(MoodAndActivitySummariesQuery request, CancellationToken cancellationToken)
        {
            var result = await _queryService.GetDatewiseMoodAndActivitySummaries(request, cancellationToken);
            return new RequestResponse<MoodAndActivitySummariesView>
            {
                Payload = result
            };
        }
    }
}
