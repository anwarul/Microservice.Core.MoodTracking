namespace RS.MF.MoodTracking.Core.Dtos
{
    public class MoodEntryDto
    {
        public Guid? ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ContentUrlInfo Image { get; set; }
    }
}