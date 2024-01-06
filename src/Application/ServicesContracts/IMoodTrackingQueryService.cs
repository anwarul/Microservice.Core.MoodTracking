using RS.MF.MoodTracking.Application.Queries;

namespace RS.MF.MoodTracking.Application.ServicesContracts;

public interface IMoodTrackingQueryService
{
    ValueTask<PagedResult<MoodTrackingLogView>> GetLogViewAsync(CommonQueryDto queryDto, CancellationToken cancellationToken);
    ValueTask<IEnumerable<MoodTrackingView>> GetMoodTrackings(UserMoodQuery queryDto, CancellationToken cancellationToken);
    ValueTask<IEnumerable<MoodSummary>> GetMoodSummaries(UserMoodSummaryQuery pagedQuery, CancellationToken cancellationToken);
    ValueTask<IEnumerable<YearlyMoodSummaryView>> GetYearlyMoodSummaries(YearlyMoodSummaryQuery pagedQuery, CancellationToken cancellationToken);
    ValueTask<MoodAndActivitySummariesView> GetDatewiseMoodAndActivitySummaries(MoodAndActivitySummariesQuery pagedQuery, CancellationToken cancellationToken);
}