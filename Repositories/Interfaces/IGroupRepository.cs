using Api.Models;

namespace Api.Repositories.Interfaces
{
    public interface IGroupRepository
    {
        Task<IEnumerable<Group>> GetAllAsync(int userId);
        Task<Group?> GetByIdAsync(int id);
        Task AddAsync(Group group);
        Task UpdateAsync(Group group);
        Task DeleteAsync(int id);
        Task<Group?> GetByIdWithMembersAsync(int id);
        Task<bool> IsUserInGroupAsync(int groupId, int userId);
    }
}
