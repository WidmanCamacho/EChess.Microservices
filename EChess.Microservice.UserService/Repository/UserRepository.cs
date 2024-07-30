using EChess.Microservice.UserService.Data;
using EChess.Microservice.UserService.Models;
using EChess.Microservice.UserService.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly UserContext _context;

    public UserRepository(UserContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _context.GetUserByIdAsync(id);
    }

    public async Task AddUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        var result = await _context.SaveChangesAsync();

        return result > 0  ? true : false;
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user != null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        return await _context.GetUserByUsernameAsync(username);
    }


}
