using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RhymingGame.Models
{
    [Table("GameQuestions")]
    public class GameQuestions
    {
        [Key]
        public long GameQuestionId { get; set; }
        public string Clue { get; set; }
        public string Answer { get; set; }
    }
}
