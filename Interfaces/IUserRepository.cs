using RhymingGame.Models;

namespace RhymingGame.Interfaces
{
    public interface IUserRepository
    {
        Task<Users> GetUserByEmailAsync(string email);
        Task<Users> GetUserByUserName(string userName);
        Task<Users> CreateUserAsync(Users user);
        Task UpdateUserAsync(Users user);
    }
}
