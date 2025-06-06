using RhymingGame.Services;
using RhymingGame.Services;
using Microsoft.AspNetCore.Mvc;

namespace RhymingGame.Controllers
{
    public class GameAuthController : Controller
    {
        private readonly AuthenticationService _authService;

        public GameAuthController(AuthenticationService authService)
        {
            _authService = authService;
        }

        public async Task<string> HandleCreateAccount(string firstName, string lastName, string email, string password)
        {
            var result = await _authService.CreateAccountAsync(firstName, lastName, email, password);

            if (result.success)
            {
                return "Account created successfully! You can now log in.";
            }
            else
            {
                return $"Error: {result.errorMessage}";
            }
        }

        public string HandleLogin(string email, string password)
        {
            var result = _authService.LoginAsync(email, password);

            if (result.success)
            {
                return $"Welcome back, {result.user.FirstName}!";
            }
            else
            {
                return $"Error: {result.errorMessage}";
            }
        }
    }
}
