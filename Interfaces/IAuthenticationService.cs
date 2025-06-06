using RhymingGame.Models;

namespace RhymingGame.Interfaces
{
    public interface IAuthenticationService
    {
        Task<(bool success, string errorMessage, Users user)> CreateAccountAsync(
            string firstName, string lastName, string email, string password);

        public (bool success, string errorMessage, Users user) LoginAsync(string email, string password);
    }
}
