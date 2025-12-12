using FileAnalysis.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileAnalysis.Infrastructure.Persistence
{
    /// <summary>
    /// Класс БД
    /// </summary>
    public class FileAnalysisDbContext : DbContext
    {
        /// <summary>
        /// Конструктор FileAnalysisDbContext
        /// </summary>
        /// <param name="options">Конфигурация подключения</param>
        public FileAnalysisDbContext(DbContextOptions<FileAnalysisDbContext> options)
            : base(options)
        {
        }
        
        /// <summary>
        /// Таблица работ
        /// </summary>
        public DbSet<Work> Works => Set<Work>();
        
        /// <summary>
        /// Таблица отчётов
        /// </summary>
        public DbSet<Report> Reports => Set<Report>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Work>(entity =>
            {
                entity.HasKey(w => w.Id);

                entity.Property(w => w.StudentId)
                    .IsRequired();

                entity.Property(w => w.AssignmentId)
                    .IsRequired();

                entity.Property(w => w.FileId)
                    .IsRequired();

                entity.Property(w => w.SubmittedAt)
                    .IsRequired();

                entity.Property(w => w.TextHash)
                    .IsRequired();

                // Work 1 - many Reports
                entity.HasMany(w => w.Reports)
                    .WithOne()
                    .HasForeignKey(r => r.WorkId);
            });

            // Report
            modelBuilder.Entity<Report>(entity =>
            {
                entity.HasKey(r => r.Id);

                entity.Property(r => r.CreatedAt)
                    .IsRequired();

                entity.Property(r => r.SimilarityScore)
                    .IsRequired();
            });
        }
    }
}