using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProjectPRN221.Models
{
    public partial class PROJECT_PRUContext : DbContext
    {
        public PROJECT_PRUContext()
        {
        }

        public PROJECT_PRUContext(DbContextOptions<PROJECT_PRUContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; } = null!;
        public virtual DbSet<EnroledCourse> EnroledCourses { get; set; } = null!;
        public virtual DbSet<Explode> Explodes { get; set; } = null!;
        public virtual DbSet<HistoryQuiz> HistoryQuizzes { get; set; } = null!;
        public virtual DbSet<Quiz> Quizzes { get; set; } = null!;
        public virtual DbSet<Token> Tokens { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var ConnectionString = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.ToTable("courses");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Categories)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("categories");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Description)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.EnrolNums).HasColumnName("enrol_nums");

                entity.Property(e => e.IsActived).HasColumnName("is_actived");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Price)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("price");

                entity.Property(e => e.Thumbnail)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("thumbnail");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Courses)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__courses__user_id__3C69FB99");
            });

            modelBuilder.Entity<EnroledCourse>(entity =>
            {
                entity.ToTable("enroled_courses");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.EnroledCourses)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__enroled_c__cours__403A8C7D");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.EnroledCourses)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__enroled_c__user___3F466844");
            });

            modelBuilder.Entity<Explode>(entity =>
            {
                entity.ToTable("explodes");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("content");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Title)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("title");

                entity.Property(e => e.Video)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("video");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Explodes)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__explodes__course__4316F928");
            });

            modelBuilder.Entity<HistoryQuiz>(entity =>
            {
                entity.ToTable("history_quizzes");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Answer).HasColumnName("answer");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.QuizzId).HasColumnName("quizz_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Quizz)
                    .WithMany(p => p.HistoryQuizzes)
                    .HasForeignKey(d => d.QuizzId)
                    .HasConstraintName("FK__history_q__quizz__49C3F6B7");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.HistoryQuizzes)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__history_q__user___48CFD27E");
            });

            modelBuilder.Entity<Quiz>(entity =>
            {
                entity.ToTable("quizzes");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Answer).HasColumnName("answer");

                entity.Property(e => e.CourseId).HasColumnName("course_id");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Option1)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("option_1");

                entity.Property(e => e.Option2)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("option_2");

                entity.Property(e => e.Option3)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("option_3");

                entity.Property(e => e.Option4)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("option_4");

                entity.Property(e => e.Question)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("question");

                entity.HasOne(d => d.Course)
                    .WithMany(p => p.Quizzes)
                    .HasForeignKey(d => d.CourseId)
                    .HasConstraintName("FK__quizzes__course___45F365D3");
            });

            modelBuilder.Entity<Token>(entity =>
            {
                entity.ToTable("tokens");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ExpiredDate)
                    .HasColumnType("datetime")
                    .HasColumnName("expired_date");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Type)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("type");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Tokens)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__tokens__user_id__398D8EEE");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("created_at");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("email");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.Password)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.Role)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("role");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime")
                    .HasColumnName("updated_at");

                entity.Property(e => e.Username)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("username");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
