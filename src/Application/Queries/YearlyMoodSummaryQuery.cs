namespace RS.MF.MoodTracking.Application.Queries;

public class YearlyMoodSummaryQuery : IQuery<RequestResponse<IEnumerable<YearlyMoodSummaryView>>>
{
    public int Year { get; set; }
}