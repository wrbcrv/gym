using Api.Dtos;

namespace Api.Services.Interfaces
{
    public interface IGroupService
    {
        Task<IEnumerable<GroupSummaryResDto>> GetAllAsync(int userId);
        Task<GroupResDto?> GetByIdAsync(int id, int userId);
        Task<GroupResDto> CreateAsync(GroupReqDto dto, int userId);
        Task<bool> UpdateAsync(int id, GroupReqDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> AddUserToGroupAsync(int groupId, int userId);
        Task<bool> RemoveUserFromGroupAsync(int groupId, int userId);
        Task<bool> UpdateAvatarUrlAsync(int groupId, string avatarUrl);
        Task<bool> UpdateCoverImageUrlAsync(int groupId, string coverUrl);
    }
}
