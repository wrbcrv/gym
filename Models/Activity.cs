namespace Api.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int GroupId { get; set; }
        public Group? Group { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public ICollection<Comment> Comments { get; set; } = [];
    }
}
