using EChess.Microservice.UserService.Models;
using EChess.Microservice.UserService.Repository.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IConfiguration _configuration;

    public UserService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _configuration = configuration;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllUsersAsync();
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        return await _userRepository.GetUserByIdAsync(id);
    }

    public async Task AddUserAsync(User user)
    {
        user.PasswordHash = HashPassword(user, string.Empty);
        await _userRepository.AddUserAsync(user);
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        user.PasswordHash = HashPassword(user, string.Empty);
        return await _userRepository.UpdateUserAsync(user);
    }

    public async Task DeleteUserAsync(int id)
    {
        await _userRepository.DeleteUserAsync(id);
    }

    public async Task<string> AuthenticateAsync(string username, string password)
    {
        var user = await _userRepository.GetUserByUsernameAsync(username);

        if (!string.IsNullOrEmpty(user.PasswordHash) && VerifyPassword(user, password))
        {
            return GenerateJwtToken(user);
        }
        
        return string.Empty;

    }

    private string HashPassword(User user, string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            user.SaltHash = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(user.SaltHash);
            }
            password = user.PasswordHash;
        }

        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: user.SaltHash,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));

        return hashed;
    }

    private bool VerifyPassword(User user, string providedPassword)
    {
        return user.PasswordHash == HashPassword(user, providedPassword);
    }

    private string GenerateJwtToken(User user)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            if (string.IsNullOrEmpty(_configuration["Jwt:Key"]))
            {
                return string.Empty;
            }

            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? string.Empty);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.Name, user.Username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }
        catch (Exception ex) 
        {
            throw new Exception(ex.Message);
        }

    }
}
