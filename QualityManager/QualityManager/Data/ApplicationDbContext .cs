using Microsoft.EntityFrameworkCore;
using QualityManager.Models;
using QualityManager.Models.Codes;

namespace QualityManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<FoodAnalysis> FoodAnalyses { get; set; }
        public DbSet<AnalysisType> AnalysisTypes { get; set; }
        public DbSet<ProcessStatus> ProcessStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region [Configuration]

            modelBuilder.Entity<FoodAnalysis>(entity =>
            {
                entity.HasOne(x => x.AnalysisType)
                        .WithMany()
                        .HasForeignKey(x => x.AnalysisTypeId)
                        .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.ProcessStatus)
                            .WithMany()
                            .HasForeignKey(x => x.ProcessStatusId)
                            .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion [Configuration]

            #region [Seed]

            modelBuilder.Entity<AnalysisType>().HasData(
                new AnalysisType { Id = 1, Name = "Microbiological Analysis" },
                new AnalysisType { Id = 2, Name = "Chemical Analysis" },
                new AnalysisType { Id = 3, Name = "Sensory Analysis" }
            );

            modelBuilder.Entity<ProcessStatus>().HasData(
                new ProcessStatus { Id = 1, Name = "Pending" },
                new ProcessStatus { Id = 2, Name = "Processing" },
                new ProcessStatus { Id = 3, Name = "Completed" }
            );

            #endregion [Seed]
        }
    }
}