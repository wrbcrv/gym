using Api.Repositories.Interfaces;

namespace Api.Utils
{
    public class PermissionValidator
    {
        private readonly IGroupRepository _groupRepository;

        public PermissionValidator(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }

        public async Task EnsureUserIsMemberAsync(int groupId, int userId)
        {
            var isMember = await _groupRepository.IsUserInGroupAsync(groupId, userId);
            if (!isMember)
                throw new UnauthorizedAccessException("Usuário não pertence ao grupo.");
        }
    }
}
