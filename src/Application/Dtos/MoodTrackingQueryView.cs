namespace RS.MF.MoodTracking.Application.Dtos
{
    public class MoodTrackingView
    {
        public DateTime Date { get; set; }
        public UserMoodTrackView UserMood { get; set; }
    }

    public class UserMoodTrackView
    {
        public IEnumerable<MoodTrackingLogBasic> MoodTrackings { get; set; }
        public IEnumerable<MoodSummary> MoodCount { get; set; }
    }

    public class UserMoodSummary
    {
        public DateTime Date { get; set; }
        public List<MoodSummary> MoodSummaries { get; set; }
    }

    public class AverageMoodSummary
    {
        public DateTime Date { get; set; }
        public MoodSummary MoodSummary { get; set; }
    }
    public class MoodSummary
    {
        public MoodEntryReference Mood { get; set; }
        public int Count { get; set; }
    }

    public class MoodAndActivitySummariesView
    {
        public List<UserMoodSummary> MoodSummaries { get; set; }
        public List<AverageMoodSummary> AverageMoodSummaries { get; set; }
        public List<ActivitySummary> TopActivities { get; set; }
        public List<MoodSummary> MoodCount { get; set; }
    }

    public class ActivitySummary
    {
        public MoodActivityReference Activity { get; set; }
        public int Count { get; set; }
    }

    #region Yearly Mood Summaries

    public record struct YearlyMoodSummaryView(int Month, List<MonthlyMoodSummaryView> DailyMoods);
    public record struct MonthlyMoodSummaryView(int Day, Guid MoodId);

    #endregion
}