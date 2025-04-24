using Api.Models;

namespace Api.DTOs
{
    public class UserResDto
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }

        public static UserResDto ValueOf(User user)
        {
            return new UserResDto
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName
            };
        }
    }
}
