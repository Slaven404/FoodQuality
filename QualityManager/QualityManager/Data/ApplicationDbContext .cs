using Microsoft.EntityFrameworkCore;
using QualityManager.Models;
using QualityManager.Models.Codes;
using Shared.Enums;

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
        public DbSet<Treshold> Tresholds { get; set; }

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

            modelBuilder.Entity<Treshold>(entity =>
            {
                entity.HasOne(x => x.AnalysisType)
                      .WithMany()
                      .HasForeignKey(x => x.AnalysisTypeId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            #endregion [Configuration]

            #region [Seed]

            modelBuilder.Entity<AnalysisType>().HasData(
                new AnalysisType { Id = (long)AnalysisTypeEnum.Microbiological, Name = "Microbiological Analysis" },
                new AnalysisType { Id = (long)AnalysisTypeEnum.Chemical, Name = "Chemical Analysis" },
                new AnalysisType { Id = (long)AnalysisTypeEnum.Sensory, Name = "Sensory Analysis" }
            );

            modelBuilder.Entity<ProcessStatus>().HasData(
                new ProcessStatus { Id = 1, Name = "Pending" },
                new ProcessStatus { Id = 2, Name = "Processing" },
                new ProcessStatus { Id = 3, Name = "Completed" }
            );

            modelBuilder.Entity<Treshold>().HasData(
                new Treshold { Id = 1, Low = 22222222, High = 77777777, AnalysisTypeId = (long)AnalysisTypeEnum.Microbiological },
                new Treshold { Id = 2, Low = 2222222, High = 7777777, AnalysisTypeId = (long)AnalysisTypeEnum.Chemical },
                new Treshold { Id = 3, Low = 222222, High = 777777, AnalysisTypeId = (long)AnalysisTypeEnum.Sensory }
            );

            #endregion [Seed]
        }
    }
}