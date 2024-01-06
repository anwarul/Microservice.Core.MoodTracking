namespace RS.MF.MoodTracking.Core.Dtos;

public class MoodTrackingDto
{
    public DateTime ActionDateTime { get; set; }
    public MoodEntryReference Mood { get; set; }
    public List<MoodActivityReference> Activity { get; set; }
    public string Note { get; set; }
}
