using Microsoft.AspNetCore.Mvc;

namespace RS.MF.MoodTracking.Application.CommandDtos
{
    public class MoodTrackingEditCommandDto
    {
        public Guid ItemId { get; set; }

        [FromBody] public JsonPatchDocument<MoodTrackingLog> Document { get; set; }
    }
}