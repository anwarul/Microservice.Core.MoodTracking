namespace RS.MF.MoodTracking.Application.Queries
{
    public class UserMoodSummaryQuery : CommonQueryDto, IQuery<RequestResponse<IEnumerable<MoodSummary>>>
    {
    }
}