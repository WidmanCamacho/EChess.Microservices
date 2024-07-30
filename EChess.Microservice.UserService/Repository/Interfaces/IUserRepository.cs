using EChess.Microservice.UserService.Models;

namespace EChess.Microservice.UserService.Repository.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
        Task<bool> UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task<User> GetUserByUsernameAsync(string username);
    }
}
