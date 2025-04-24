using Api.Dtos;
using Api.DTOs;

namespace Api.Services.Interfaces
{
    public interface IActivityService
    {
        Task<ActivityResDto> GetByIdAsync(int groupId, int activityId, int userId);
        Task<IEnumerable<ActivityResDto>> GetAllByGroupIdAsync(int groupId, int userId, int pageNumber, int pageSize);
        Task<ActivityResDto> CreateAsync(int groupId, int userId, ActivityReqDto dto);
        Task<bool> UpdateAsync(int activityId, int groupId, int userId, ActivityReqDto dto);
        Task<bool> DeleteAsync(int activityId, int groupId);
        Task<CommentResDto> CreateCommentAsync(int groupId, int activityId, int userId, CommentReqDto dto);
        Task<IEnumerable<ActivityRankingDto>> GetRankingByGroupIdAsync(int groupId, int userId);
    }
}
