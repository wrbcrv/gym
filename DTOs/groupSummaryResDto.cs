using Api.Models;

namespace Api.Dtos
{
    public class GroupSummaryResDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int MemberCount { get; set; }

        public static GroupSummaryResDto ValueOf(Group group)
        {
            return new GroupSummaryResDto
            {
                Id = group.Id,
                Name = group.Name,
                MemberCount = group.Members?.Count ?? 0
            };
        }
    }
}
