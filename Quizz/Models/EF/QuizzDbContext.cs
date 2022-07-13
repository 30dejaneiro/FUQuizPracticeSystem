using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Quizz.Models.EF
{
    public partial class QuizzDbContext : DbContext
    {
        public QuizzDbContext()
            : base("name=QuizzDbContext2")
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<BankQuestion> BankQuestions { get; set; }
        public virtual DbSet<Exam> Exams { get; set; }
        public virtual DbSet<ExamsQue> ExamsQues { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Score> Scores { get; set; }
        public virtual DbSet<Subject> Subjects { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExamsQue>()
                .Property(e => e.answer)
                .IsUnicode(false);

            modelBuilder.Entity<Question>()
                .Property(e => e.answer)
                .IsUnicode(false);
        }
    }
}
