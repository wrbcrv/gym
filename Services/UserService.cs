using Api.DTOs;
using Api.Models;
using Api.Repositories;
using Api.Repositories.Interfaces;
using Api.Services.Interfaces;

namespace Api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<UserResDto>> GetAllAsync()
        {
            var users = await _repository.GetAllAsync();
            return users.Select(UserResDto.ValueOf);
        }

        public async Task<UserResDto?> GetByIdAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            return user != null ? UserResDto.ValueOf(user) : null;
        }

        public async Task<UserResDto> CreateAsync(UserReqDto userDto)
        {
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userDto.Password);

            var user = new User
            {
                FullName = userDto.FullName,
                UserName = userDto.UserName,
                Password = hashedPassword
            };

            await _repository.AddAsync(user);
            return UserResDto.ValueOf(user);
        }

        public async Task<bool> UpdateAsync(int id, UserReqDto userDto)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            existing.FullName = userDto.FullName;
            existing.UserName = userDto.UserName;

            if (!string.IsNullOrWhiteSpace(userDto.Password))
            {
                existing.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            }

            await _repository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) return false;

            await _repository.DeleteAsync(id);
            return true;
        }

        public async Task<UserResDto?> GetByUsernameAsync(string username, string password)
        {
            var user = await _repository.GetByUsernameAsync(username);
            if (user == null) return null;

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);
            return isPasswordValid ? UserResDto.ValueOf(user) : null;
        }
    }
}
