using Api.Dtos;
using Api.DTOs;
using Api.Models;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using Api.Utils;

namespace Api.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;
        private readonly PermissionValidator _permissionValidator;

        public ActivityService(
            IActivityRepository activityRepository,
            IGroupRepository groupRepository,
            IUserRepository userRepository,
            PermissionValidator permissionValidator)
        {
            _activityRepository = activityRepository;
            _groupRepository = groupRepository;
            _userRepository = userRepository;
            _permissionValidator = permissionValidator;
        }

        public async Task<ActivityResDto> GetByIdAsync(int groupId, int activityId, int userId)
        {
            await _permissionValidator.EnsureUserIsMemberAsync(groupId, userId);

            var activity = await _activityRepository.GetByIdAsync(activityId) ?? throw new KeyNotFoundException("Atividade não encontrada.");

            if (activity.GroupId != groupId)
                throw new UnauthorizedAccessException("Esta atividade não pertence ao grupo informado.");

            return ActivityResDto.ValueOf(activity);
        }

        public async Task<IEnumerable<ActivityResDto>> GetAllByGroupIdAsync(int groupId, int userId, int pageNumber, int pageSize)
        {
            await _permissionValidator.EnsureUserIsMemberAsync(groupId, userId);
            var activities = await _activityRepository.GetAllByGroupIdAsync(groupId, pageNumber, pageSize);
            return activities.Select(ActivityResDto.ValueOf);
        }

        public async Task<ActivityResDto> CreateAsync(int groupId, int userId, ActivityReqDto dto)
        {
            await _permissionValidator.EnsureUserIsMemberAsync(groupId, userId);

            if (dto.Date > DateOnly.FromDateTime(DateTime.UtcNow))
                throw new InvalidOperationException("Não é permitido adicionar atividades em datas futuras.");

            var activity = new Activity
            {
                Title = dto.Title,
                Description = dto.Description,
                Date = dto.Date,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                GroupId = groupId,
                UserId = userId
            };

            await _activityRepository.AddAsync(activity);
            return ActivityResDto.ValueOf(activity);
        }

        public async Task<bool> UpdateAsync(int activityId, int groupId, int userId, ActivityReqDto dto)
        {
            var activity = await _activityRepository.GetByIdAsync(activityId);
            if (activity == null || activity.GroupId != groupId) return false;

            activity.Title = dto.Title;
            activity.Description = dto.Description;
            activity.Date = dto.Date;
            activity.StartTime = dto.StartTime;
            activity.EndTime = dto.EndTime;
            activity.UserId = userId;

            return await _activityRepository.UpdateAsync(groupId, activity);
        }

        public async Task<bool> DeleteAsync(int activityId, int groupId)
        {
            var activity = await _activityRepository.GetByIdAsync(activityId);
            if (activity == null || activity.GroupId != groupId) return false;

            return await _activityRepository.DeleteAsync(activityId, groupId);
        }

        public async Task<CommentResDto> CreateCommentAsync(int groupId, int activityId, int userId, CommentReqDto dto)
        {
            await _permissionValidator.EnsureUserIsMemberAsync(groupId, userId);

            var activity = await _activityRepository.GetByIdAsync(activityId) ?? throw new KeyNotFoundException("Atividade não encontrada.");

            if (activity.GroupId != groupId)
                throw new UnauthorizedAccessException("A atividade não pertence ao grupo informado.");

            var user = await _userRepository.GetByIdAsync(userId) ?? throw new KeyNotFoundException("Usuário não encontrado.");

            var comment = new Comment
            {
                Content = dto.Content,
                ActivityId = activityId,
                UserId = userId,
                User = user
            };

            await _activityRepository.AddCommentAsync(comment);
            return CommentResDto.ValueOf(comment);
        }

        public async Task<IEnumerable<ActivityRankingDto>> GetRankingByGroupIdAsync(int groupId, int userId)
        {
            await _permissionValidator.EnsureUserIsMemberAsync(groupId, userId);

            var result = await _activityRepository.GetUserActivityCountsByGroupAsync(groupId);

            return result.Select(x => new ActivityRankingDto
            {
                UserId = x.UserId,
                FullName = x.FullName,
                Count = x.Count
            });
        }

    }
}
