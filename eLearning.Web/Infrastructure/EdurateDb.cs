using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using edurate.Web.Models;

namespace edurate.Web.Infrastructure
{
    public class EdurateDb : DbContext
    {
        public EdurateDb()
            : base("DefaultConnection")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<LocalizedCategory> LocalizedCategories { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<Quiz> Quizes { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionAnswer> QuestionAnswers { get; set; }
        public DbSet<QuizAttempt> QuizAttempts { get; set; }
        public DbSet<QuizAttemptAnswer> QuizAttemptAnswers { get; set; }

        public DbSet<UsersInCategory> UsersInCategory { get; set; }
        public DbSet<CourseRating> CourseRatings { get; set; }
        public DbSet<ArticleRating> ArticleRatings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Entity<User>()
                .HasMany(u => u.Courses)
                .WithMany(c => c.Students)
                .Map(m => m.MapLeftKey("UserId")
                    .MapRightKey("CourseId")
                    .ToTable("UsersInCourse"));

            modelBuilder.Entity<Question>().Property(q => q.QuestionType).HasColumnName("QuestionType");
            modelBuilder.Entity<QuizAttempt>().Property(e => e.StartedTime).HasColumnType("datetime2");
            modelBuilder.Entity<QuizAttempt>().Property(e => e.FinishedTime).HasColumnType("datetime2");
            modelBuilder.Entity<User>().Property(e => e.DateOfBirth).HasColumnType("datetime2");
            modelBuilder.Entity<Article>().Property(e => e.CreatedDate).HasColumnType("datetime2");
            modelBuilder.Entity<Article>().Property(e => e.LastModifiedDate).HasColumnType("datetime2");

            base.OnModelCreating(modelBuilder);
        }
    }
}