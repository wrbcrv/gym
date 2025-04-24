using Api.Models;

namespace Api.Dtos
{
    public class CommentResDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int UserId { get; set; }
        public string? UserFullName { get; set; } = string.Empty;

        public static CommentResDto ValueOf(Comment comment)
        {
            return new CommentResDto
            {
                Id = comment.Id,
                Content = comment.Content,
                CreatedAt = comment.CreatedAt,
                UserId = comment.User.Id,
                UserFullName = comment.User.FullName
            };
        }
    }
}
