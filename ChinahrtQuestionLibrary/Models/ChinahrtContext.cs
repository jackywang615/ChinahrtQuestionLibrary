using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChinahrtQuestionLibrary.Models
{
    public class ChinahrtContext : DbContext
    {
        public DbSet<Question> Questions { get; set; }

        public ChinahrtContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            QuestionConfiguration(modelBuilder.Entity<Question>());
            AnswerConfiguration(modelBuilder.Entity<Answer>());
        }

        private void AnswerConfiguration(EntityTypeBuilder<Answer> builder)
        {
            builder.ToTable("Answers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Result).HasMaxLength(500).IsRequired();
            builder.Property(x => x.IsRight).IsRequired();
            builder.Property<Guid>("Question_Id").IsRequired();
        }

        private void QuestionConfiguration(EntityTypeBuilder<Question> builder)
        {
            builder.ToTable("Questions");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Title).HasMaxLength(500).IsRequired();
            builder.HasMany(x => x.Answers).WithOne().HasForeignKey("Question_Id").OnDelete(DeleteBehavior.Cascade);
        }
    }
}