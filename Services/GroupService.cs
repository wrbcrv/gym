using Api.Dtos;
using Api.Models;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;
using Api.Utils;

namespace Api.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IUserRepository _userRepository;
        private readonly PermissionValidator _permissionValidator;

        public GroupService(
            IGroupRepository groupRepository,
            IUserRepository userRepository,
            PermissionValidator permissionValidator)
        {
            _groupRepository = groupRepository;
            _userRepository = userRepository;
            _permissionValidator = permissionValidator;
        }

        public async Task<IEnumerable<GroupSummaryResDto>> GetAllAsync(int userId)
        {
            var groups = await _groupRepository.GetAllAsync(userId);
            return groups.Select(GroupSummaryResDto.ValueOf);
        }

        public async Task<GroupResDto?> GetByIdAsync(int id, int userId)
        {
            await _permissionValidator.EnsureUserIsMemberAsync(id, userId);
            var group = await _groupRepository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Grupo não encontrado.");
            return GroupResDto.ValueOf(group);
        }

        public async Task<GroupResDto> CreateAsync(GroupReqDto dto, int userId)
        {
            var members = new List<User>();
            var admin = await _userRepository.GetByIdAsync(userId) ?? throw new Exception("Usuário criador não encontrado.");
            members.Add(admin);

            if (dto.MemberIds != null && dto.MemberIds.Any())
            {
                foreach (var id in dto.MemberIds.Distinct())
                {
                    if (id == userId) continue;
                    var user = await _userRepository.GetByIdAsync(id);
                    if (user != null && !members.Any(m => m.Id == user.Id))
                    {
                        members.Add(user);
                    }
                }
            }

            var group = new Group
            {
                Name = dto.Name,
                CreatedAt = DateTime.UtcNow,
                Members = members,
                AdminId = userId
            };

            await _groupRepository.AddAsync(group);
            return GroupResDto.ValueOf(group);
        }

        public async Task<bool> UpdateAsync(int id, GroupReqDto dto)
        {
            var group = await _groupRepository.GetByIdAsync(id);
            if (group == null) return false;

            group.Name = dto.Name ?? group.Name;

            var currentAdminId = group.AdminId;

            group.Members.Clear();

            if (dto.MemberIds != null && dto.MemberIds.Count != 0)
            {
                foreach (var userId in dto.MemberIds.Distinct())
                {
                    var user = await _userRepository.GetByIdAsync(userId);
                    if (user != null && !group.Members.Any(m => m.Id == user.Id))
                    {
                        group.Members.Add(user);
                    }
                }
            }

            var adminUser = await _userRepository.GetByIdAsync(currentAdminId);
            if (adminUser != null && !group.Members.Any(m => m.Id == adminUser.Id))
            {
                group.Members.Add(adminUser);
            }

            group.AdminId = currentAdminId;

            await _groupRepository.UpdateAsync(group);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var group = await _groupRepository.GetByIdAsync(id);
            if (group == null) return false;

            await _groupRepository.DeleteAsync(id);
            return true;
        }

        public async Task<bool> AddUserToGroupAsync(int groupId, int userId)
        {
            var group = await _groupRepository.GetByIdAsync(groupId);
            if (group == null)
                return false;

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            if (group.Members.Any(m => m.Id == userId))
                return false;

            group.Members.Add(user);
            await _groupRepository.UpdateAsync(group);

            return true;
        }

        public async Task<bool> RemoveUserFromGroupAsync(int groupId, int userId)
        {
            var group = await _groupRepository.GetByIdAsync(groupId);
            if (group == null)
                return false;

            var user = group.Members.FirstOrDefault(m => m.Id == userId);
            if (user == null)
                return false;

            group.Members.Remove(user);
            await _groupRepository.UpdateAsync(group);

            return true;
        }

        public async Task<bool> UpdateAvatarUrlAsync(int groupId, string avatarUrl)
        {
            var group = await _groupRepository.GetByIdAsync(groupId);
            if (group == null) return false;

            group.AvatarUrl = avatarUrl;
            await _groupRepository.UpdateAsync(group);
            return true;
        }

        public async Task<bool> UpdateCoverImageUrlAsync(int groupId, string coverUrl)
        {
            var group = await _groupRepository.GetByIdAsync(groupId);
            if (group == null) return false;

            group.CoverImageUrl = coverUrl;
            await _groupRepository.UpdateAsync(group);
            return true;
        }
    }
}
