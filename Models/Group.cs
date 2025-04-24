namespace Api.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int AdminId { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? AvatarUrl { get; set; }
        public ICollection<User> Members { get; set; } = [];
        public ICollection<Activity> Activities { get; set; } = [];
    }
}
