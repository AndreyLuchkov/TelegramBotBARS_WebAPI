using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace TelegramBotBARS_WebAPI.Models
{
    public partial class TGBot_BARS_DbContext : DbContext
    {
        public TGBot_BARS_DbContext()
        {
        }

        public TGBot_BARS_DbContext(DbContextOptions<TGBot_BARS_DbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ControlEvent> ControlEvents { get; set; } = null!;
        public virtual DbSet<MissedLessonRecord> MissedLessonRecords { get; set; } = null!;
        public virtual DbSet<Statement> Statements { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<ControlEvent>(entity =>
            {
                entity.HasKey(e => new { e.Number, e.StatementId, e.Score })
                    .HasName("control_events_pkey");

                entity.ToTable("control_events");

                entity.Property(e => e.Number).HasColumnName("number");

                entity.Property(e => e.StatementId).HasColumnName("statement_id");

                entity.Property(e => e.Score).HasColumnName("score");

                entity.Property(e => e.Name).HasColumnName("name");

                entity.Property(e => e.ScoreAddDate)
                    .HasColumnType("timestamp without time zone")
                    .HasColumnName("score_add_date");

                entity.Property(e => e.ScoreStatus)
                    .HasMaxLength(50)
                    .HasColumnName("score_status");

                entity.Property(e => e.WeekNumber).HasColumnName("week_number");

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.HasOne(d => d.Statement)
                    .WithMany(p => p.ControlEvents)
                    .HasForeignKey(d => d.StatementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("control_events_statementId_fkey");
            });

            modelBuilder.Entity<MissedLessonRecord>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.StatementId, e.LessonTime, e.LessonDate, e.LessonType })
                    .HasName("missed_lesson_records_pkey");

                entity.ToTable("missed_lesson_records");

                entity.HasIndex(e => e.StatementId, "fki_missed_lesson_records_statementId_fkey");

                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.Property(e => e.StatementId).HasColumnName("statement_id");

                entity.Property(e => e.LessonTime)
                    .HasMaxLength(20)
                    .HasColumnName("lesson_time");

                entity.Property(e => e.LessonDate).HasColumnName("lesson_date");

                entity.Property(e => e.LessonType)
                    .HasMaxLength(50)
                    .HasColumnName("lesson_type");

                entity.Property(e => e.Reason).HasColumnName("reason");

                entity.HasOne(d => d.Statement)
                    .WithMany(p => p.MissedLessonRecords)
                    .HasForeignKey(d => d.StatementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("missed_lesson_records_statementId_fkey");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.MissedLessonRecords)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("missed_lesson_records_studentId_fkey");
            });

            modelBuilder.Entity<Statement>(entity =>
            {
                entity.ToTable("statements");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.AttestationScore).HasColumnName("attestation_score");

                entity.Property(e => e.AttestationType)
                    .HasMaxLength(30)
                    .HasColumnName("attestation_type");

                entity.Property(e => e.Discipline).HasColumnName("discipline");

                entity.Property(e => e.ResultScore).HasColumnName("result_score");

                entity.Property(e => e.Semester)
                    .HasMaxLength(27)
                    .HasColumnName("semester");

                entity.Property(e => e.SemesterScore).HasColumnName("semester_score");

                entity.Property(e => e.StudentId).HasColumnName("student_id");

                entity.Property(e => e.Teacher)
                    .HasMaxLength(150)
                    .HasColumnName("teacher");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Statements)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("statements_student_id_fkey");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("students");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Login)
                    .HasMaxLength(30)
                    .HasColumnName("login");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
