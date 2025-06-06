using Microsoft.AspNetCore.Mvc;
using RhymingGame.Extensions;
using RhymingGame.Models;
using System.Diagnostics;
using RhymingGame.Database;
using RhymingGame.Interfaces;

namespace RhymingGame.Controllers
{
    public class GameController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DBContext _dbContext;
        private readonly IAuthenticationService _authService;

        public GameController(ILogger<HomeController> logger, DBContext dBContext, IAuthenticationService authService)
        {
            _logger = logger;
            _dbContext = dBContext;
            _authService = authService;
        }

        public IActionResult Index()
        {
            var questions = GetQuestions();
            HttpContext.Session.SetObject("GameQuestions", questions);
            return View();
        }

        public IActionResult GetNextQuestion(int questionNumber)
        {
            // Retrieve the questions from session
            var questions = HttpContext.Session.GetObject<List<GameQuestions>>("GameQuestions");
            //var questions = Extensions.SessionExtensions.GetObject<List<GameQuestions>>(HttpContext.Session, "GameQuestions");

            if (questions == null || !questions.Any())

            {
                return Json(new { message = "No questions available or game over." });
            }

            // Get the next question (e.g., the first one in the list)
            var nextQuestion = questions[questionNumber];

            //// Remove the question from the list after it's asked
            //questions.RemoveAt(0);
            //HttpContext.Session.Set("GameQuestions", questions);

            return Json(nextQuestion);
        }

        [HttpPost]
        public IActionResult CheckAnswer(string answer, int gameQuestionId)
        {
            // Retrieve the questions from session
            var currentQuestion = HttpContext.Session.GetObject<List<GameQuestions>>("GameQuestions")?.FirstOrDefault(x => x.GameQuestionId == gameQuestionId);
            //var questions = Extensions.SessionExtensions.GetObject<List<GameQuestions>>(HttpContext.Session, "GameQuestions");

            if (currentQuestion == null)
            {
                return Json(new { message = "No questions available or game over." });
            }

            // Check if the user's answer is a flipped version of the correct answer
            var flippedCorrectAnswer = string.Join(" ", currentQuestion.Answer.Split(' ').Reverse());
            if (answer.ToLower() == flippedCorrectAnswer.ToLower())
            {
                return Json(new { message = "Flip It" });
            }

            // Get the current question
            //var currentQuestion = questions[questionNumber];

            // Check if the answer is correct
            bool isCorrect = string.Equals(currentQuestion.Answer.ToLower(), answer.ToLower(), StringComparison.OrdinalIgnoreCase);

            // Record the question history
            //var questionHistory = new GameQuestionHistory
            //{
            //    GameQuestionId = currentQuestion.GameQuestionId,
            //    UserId = 1, // Replace with actual user ID logic
            //    DateAsked = DateTime.Now,
            //    AnsweredCorrectly = isCorrect
            //};

            //_dbContext.GameQuestionHistory.Add(questionHistory);
            //_dbContext.SaveChanges();

            return Json(new { message = isCorrect ? "Correct!" : "Incorrect. Try again." });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        private List<GameQuestions> GetQuestions()
        {
            var topQuestions = _dbContext.GameQuestions
            .Where(q => !_dbContext.GameQuestionHistory
                            .Any(h => h.GameQuestionId == q.GameQuestionId))
            .Take(5)
            .ToList();

            return topQuestions;
        }
    }
}
