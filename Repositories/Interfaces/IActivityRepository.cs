using Api.Models;

namespace Api.Repositories.Interfaces
{
    public interface IActivityRepository
    {
        Task<Activity?> GetByIdAsync(int id);
        Task<IEnumerable<Activity>> GetAllByGroupIdAsync(int groupId, int pageNumber, int pageSize);
        Task AddAsync(Activity activity);
        Task<bool> UpdateAsync(int groupId, Activity activity);
        Task<bool> DeleteAsync(int activityId, int groupId);
        Task AddCommentAsync(Comment comment);
        Task<IEnumerable<(int UserId, string? FullName, int Count)>> GetUserActivityCountsByGroupAsync(int groupId);
    }
}
