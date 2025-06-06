using RhymingGame.Database;
using RhymingGame.Models;
using RhymingGame.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using RhymingGame.Services;
using RhymingGame.Interfaces;
using RhymingGame.Models;

namespace RhymingGame.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DBContext _dbContext;
        private readonly IAuthenticationService _authService;

        public HomeController(ILogger<HomeController> logger, DBContext dBContext, IAuthenticationService authService)
        {
            _logger = logger;
            _dbContext = dBContext;
            _authService = authService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(Users model)
        {
            var result = _authService.LoginAsync(model.Email, model.PasswordHash);
            if (result.success)
            {
                // Set authentication cookie/session
                return RedirectToAction("Game", "Home");
            }

            TempData["Error"] = result.errorMessage;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Users model)
        {
            var result = await _authService.CreateAccountAsync(
                model.FirstName, model.LastName, model.Email, model.PasswordHash);

            if (result.success)
            {
                TempData["Success"] = "Account created successfully! Please log in.";
                return View();
            }

            TempData["Error"] = result.errorMessage;
            return View();
        }
    }
}
