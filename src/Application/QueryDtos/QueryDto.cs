namespace RS.MF.MoodTracking.Application.QueryDtos
{
    public class CommonQueryDto : PagedQueryBase
    {
        public Guid? UserId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool HasPagination { get; set; }
    }
}