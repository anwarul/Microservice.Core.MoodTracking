namespace RS.MF.MoodTracking.Application.Queries
{
    public class UserMoodQuery : CommonQueryDto, IQuery<RequestResponse<IEnumerable<MoodTrackingView>>>
    {
        public string Dates { get; set; }
    }
}