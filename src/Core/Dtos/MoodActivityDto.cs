namespace RS.MF.MoodTracking.Core.Dtos;

public class MoodActivityDto
{
    public Guid? ItemId { get; set; }
    public string Name { get; set; }
    //public string Description { get; set; }
    public string ImageUrl { get; set; }
    public MoodActivityGroupReference ActivityGroup { get; set; }
}
