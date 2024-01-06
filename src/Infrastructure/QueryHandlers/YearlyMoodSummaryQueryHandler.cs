using Microsoft.Extensions.Logging;
using RedlimeSolutions.Microservice.Framework.Core.Queries;
using RS.MF.MoodTracking.Application.Dtos;
using RS.MF.MoodTracking.Application.Queries;

namespace RS.MF.MoodTracking.Infrastructure.QueryHandlers;

public class YearlyMoodSummaryQueryHandler : IQueryHandler<YearlyMoodSummaryQuery, RequestResponse<IEnumerable<YearlyMoodSummaryView>>>
{
    private readonly IMoodTrackingQueryService _queryService;

    public YearlyMoodSummaryQueryHandler(
        IMoodTrackingQueryService queryService
        )
    {
        _queryService = queryService;
    }


    public async Task<RequestResponse<IEnumerable<YearlyMoodSummaryView>>> Handle(YearlyMoodSummaryQuery request, CancellationToken cancellationToken)
    {
        return new RequestResponse<IEnumerable<YearlyMoodSummaryView>>
        {
            Payload = await _queryService.GetYearlyMoodSummaries(request, cancellationToken)
        };
    }
}
