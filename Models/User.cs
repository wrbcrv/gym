namespace Api.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public ICollection<Group> Groups { get; set; } = [];
        public ICollection<Activity> Activities { get; set; } = [];
    }
}
