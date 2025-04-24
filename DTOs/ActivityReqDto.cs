namespace Api.Dtos
{
    public class ActivityReqDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}
