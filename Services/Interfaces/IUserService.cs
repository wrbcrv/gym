using Api.DTOs;

namespace Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResDto>> GetAllAsync();
        Task<UserResDto?> GetByIdAsync(int id);
        Task<UserResDto> CreateAsync(UserReqDto userDto);
        Task<bool> UpdateAsync(int id, UserReqDto userDto);
        Task<bool> DeleteAsync(int id);
        Task<UserResDto?> GetByUsernameAsync(string username, string password);
    }
}
