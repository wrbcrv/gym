using Api.Models;

namespace Api.Dtos
{
    public class MemberResDto
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Role { get; set; }

        public static MemberResDto ValueOf(User user, int adminId)
        {
            return new MemberResDto
            {
                Id = user.Id,
                FullName = user.FullName,
                UserName = user.UserName,
                Role = user.Id == adminId ? "Admin" : null
            };
        }
    }
}
