using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RhymingGame.Models
{
    [Table("GameQuestionHistory")]
    public class GameQuestionHistory
    {
        [Key]
        public long GameQuestionHistoryId { get; set; }
        public long GameQuestionId { get; set; }
        public long UserId { get; set; }
        public DateTime DateAsked { get; set; }
        public bool AnsweredCorrectly { get; set; }
    }
}
