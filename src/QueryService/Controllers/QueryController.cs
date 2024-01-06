using RedlimeSolutions.Microservice.Framework.Core.Queries;
using RS.MF.MoodTracking.Application.Dtos;
using RS.MF.MoodTracking.Application.Queries;
using RS.MF.MoodTracking.Application.QueryDtos;
using RS.MFramework.CacheHeaders;

namespace RS.MF.ServiceName.HostService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 1000)]
    [HttpCacheValidation(MustRevalidate = true)]
    public class QueryController : ControllerBase
    {
        private readonly IQueryBus _queryBus;


        public QueryController(
            IQueryBus queryBus
            )
        {
            _queryBus = queryBus;
        }

        [HttpGet("user-mood-history")]
        [AnyonomusEndPoint]
        public async ValueTask<RequestResponse<IEnumerable<MoodTrackingView>>> UserMoodHistory([FromQuery] UserMoodQuery queryDto, CancellationToken cancellationToken)
        {
            var result = await _queryBus.Send(queryDto, cancellationToken);
            return result;
        }

        [HttpGet("user-mood-summary")]
        [AnyonomusEndPoint]
        public async ValueTask<RequestResponse<IEnumerable<MoodSummary>>> UserMoodSummary([FromQuery] CommonQueryDto queryDto, CancellationToken cancellationToken)
        {
            var query = Mapping.Map<CommonQueryDto, UserMoodSummaryQuery>(queryDto);
            var result = await _queryBus.Send(query, cancellationToken);
            return result;
        }

        [HttpGet("mood-and-activity-summary")]
        [AnyonomusEndPoint]
        public async ValueTask<RequestResponse<MoodAndActivitySummariesView>> MoodAndActivitySummary([FromQuery] MoodAndActivitySummariesQueryDto queryDto, CancellationToken cancellationToken)
        {
            var query = Mapping.Map<MoodAndActivitySummariesQueryDto, MoodAndActivitySummariesQuery>(queryDto);
            var result = await _queryBus.Send(query, cancellationToken);
            return result;
        }

        [HttpGet("yearly-mood-summary")]
        [AnyonomusEndPoint]
        public async ValueTask<RequestResponse<IEnumerable<YearlyMoodSummaryView>>> YearlyMoodSummary([FromQuery] YearlyMoodSummaryQuery query, CancellationToken cancellationToken)
        {
            var result = await _queryBus.Send(query, cancellationToken);
            return result;
        }

    }
}