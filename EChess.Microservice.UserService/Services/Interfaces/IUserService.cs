using EChess.Microservice.UserService.Models;

public interface IUserService
{
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<User> GetUserByIdAsync(int id);
    Task AddUserAsync(User user);
    Task<bool> UpdateUserAsync(User user);
    Task DeleteUserAsync(int id);
    Task<string> AuthenticateAsync(string username, string password);
}
