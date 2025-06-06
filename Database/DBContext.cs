using RhymingGame.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace RhymingGame.Database
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        public DbSet<GameQuestions> GameQuestions { get; set; }

        public DbSet<GameQuestionHistory> GameQuestionHistory { get; set; }

        public DbSet<Users> Users { get; set; }

    }
}
