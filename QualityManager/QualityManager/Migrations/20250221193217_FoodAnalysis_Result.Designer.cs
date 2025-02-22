﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using QualityManager.Data;

#nullable disable

namespace QualityManager.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250221193217_FoodAnalysis_Result")]
    partial class FoodAnalysis_Result
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("QualityManager.Models.Codes.AnalysisType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("AnalysisTypes");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Name = "Microbiological Analysis"
                        },
                        new
                        {
                            Id = 2L,
                            Name = "Chemical Analysis"
                        },
                        new
                        {
                            Id = 3L,
                            Name = "Sensory Analysis"
                        });
                });

            modelBuilder.Entity("QualityManager.Models.Codes.ProcessStatus", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("ProcessStatuses");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            Name = "Pending"
                        },
                        new
                        {
                            Id = 2L,
                            Name = "Processing"
                        },
                        new
                        {
                            Id = 3L,
                            Name = "Completed"
                        });
                });

            modelBuilder.Entity("QualityManager.Models.FoodAnalysis", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("AnalysisTypeId")
                        .HasColumnType("bigint");

                    b.Property<string>("FoodName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("ProcessStatusId")
                        .HasColumnType("bigint");

                    b.Property<string>("Result")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("Id");

                    b.HasIndex("AnalysisTypeId");

                    b.HasIndex("ProcessStatusId");

                    b.ToTable("FoodAnalyses");
                });

            modelBuilder.Entity("QualityManager.Models.FoodAnalysis", b =>
                {
                    b.HasOne("QualityManager.Models.Codes.AnalysisType", "AnalysisType")
                        .WithMany()
                        .HasForeignKey("AnalysisTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("QualityManager.Models.Codes.ProcessStatus", "ProcessStatus")
                        .WithMany()
                        .HasForeignKey("ProcessStatusId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("AnalysisType");

                    b.Navigation("ProcessStatus");
                });
#pragma warning restore 612, 618
        }
    }
}
