namespace Api.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int ActivityId { get; set; }
        public Activity Activity { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
