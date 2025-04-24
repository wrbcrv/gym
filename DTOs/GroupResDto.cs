using Api.DTOs;
using Api.Models;

namespace Api.Dtos
{
    public class GroupResDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<MemberResDto> Members { get; set; } = [];

        public static GroupResDto ValueOf(Group group)
        {
            return new GroupResDto
            {
                Id = group.Id,
                Name = group.Name,
                CreatedAt = group.CreatedAt,
                Members = [.. group.Members.Select(user => MemberResDto.ValueOf(user, group.AdminId))]
            };
        }
    }
}
