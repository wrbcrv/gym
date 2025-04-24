namespace Api.Dtos
{
    public class GroupReqDto
    {
        public string? Name { get; set; }
        public List<int>? MemberIds { get; set; } = [];
    }
}
