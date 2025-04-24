using Api.Models;

namespace Api.Dtos
{
    public class ActivityResDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? UserFullName { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string Date { get; set; } = string.Empty;
        public string StartTime { get; set; } = string.Empty;
        public string EndTime { get; set; } = string.Empty;

        public List<CommentResDto> Comments { get; set; } = [];

        public static ActivityResDto ValueOf(Activity activity)
        {
            return new ActivityResDto
            {
                Id = activity.Id,
                UserId = activity.UserId,
                UserFullName = activity.User?.FullName,
                Title = activity.Title,
                Description = activity.Description,
                Date = activity.Date.ToString("dd/MM/yyyy"),
                StartTime = activity.StartTime.ToString("HH:mm"),
                EndTime = activity.EndTime.ToString("HH:mm"),
                Comments = activity.Comments?
                    .OrderByDescending(c => c.CreatedAt)
                    .Select(CommentResDto.ValueOf)
                    .ToList() ?? []
            };
        }
    }
}
