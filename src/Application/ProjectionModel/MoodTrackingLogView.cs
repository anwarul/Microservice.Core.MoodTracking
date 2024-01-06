namespace RS.MF.MoodTracking.Application.ProjectionModel;

[BsonIgnoreExtraElements]
public class MoodTrackingLogView
{
    [BsonElement("_id")]
    public Guid ItemId { get; set; }
    public DateTime ActionDateTime { get; set; }
    public MoodEntryReference Mood { get; set; }
    public List<MoodActivityReference> Activity { get; set; }
    public string Note { get; set; }
    public DateTime CreateDate { get; set; }
    public Guid CreatedByUserId { get; set; }
}
