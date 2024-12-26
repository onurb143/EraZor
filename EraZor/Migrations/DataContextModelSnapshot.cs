﻿// <auto-generated />
using System;
using EraZor.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EraZor.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EraZor.Models.Disk", b =>
                {
                    b.Property<int>("DiskID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("DiskID"));

                    b.Property<int>("Capacity")
                        .HasColumnType("integer");

                    b.Property<string>("Manufacturer")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Path")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("SerialNumber")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("DiskID");

                    b.HasIndex("SerialNumber")
                        .IsUnique();

                    b.ToTable("Disks");
                });

            modelBuilder.Entity("EraZor.Models.LogEntry", b =>
                {
                    b.Property<int>("LogID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("LogID"));

                    b.Property<string>("Message")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("WipeJobId")
                        .HasColumnType("integer");

                    b.HasKey("LogID");

                    b.HasIndex("WipeJobId");

                    b.ToTable("LogEntries");
                });

            modelBuilder.Entity("EraZor.Models.WipeMethod", b =>
                {
                    b.Property<int>("WipeMethodID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("WipeMethodID"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("OverwritePass")
                        .HasColumnType("integer");

                    b.HasKey("WipeMethodID");

                    b.ToTable("WipeMethods");

                    b.HasData(
                        new
                        {
                            WipeMethodID = 1,
                            Description = "",
                            Name = "DoD 5220.22-M",
                            OverwritePass = 3
                        },
                        new
                        {
                            WipeMethodID = 2,
                            Description = "",
                            Name = "NIST 800-88 Clear",
                            OverwritePass = 1
                        },
                        new
                        {
                            WipeMethodID = 3,
                            Description = "",
                            Name = "NIST 800-88 Purge",
                            OverwritePass = 1
                        },
                        new
                        {
                            WipeMethodID = 4,
                            Description = "",
                            Name = "Gutmann",
                            OverwritePass = 35
                        },
                        new
                        {
                            WipeMethodID = 5,
                            Description = "",
                            Name = "Random Data",
                            OverwritePass = 1
                        },
                        new
                        {
                            WipeMethodID = 6,
                            Description = "",
                            Name = "Write Zero",
                            OverwritePass = 1
                        },
                        new
                        {
                            WipeMethodID = 7,
                            Description = "",
                            Name = "Write One",
                            OverwritePass = 1
                        },
                        new
                        {
                            WipeMethodID = 8,
                            Description = "",
                            Name = "Schneider",
                            OverwritePass = 7
                        },
                        new
                        {
                            WipeMethodID = 9,
                            Description = "",
                            Name = "Bruce Force",
                            OverwritePass = 10
                        },
                        new
                        {
                            WipeMethodID = 10,
                            Description = "",
                            Name = "Quick Format",
                            OverwritePass = 1
                        },
                        new
                        {
                            WipeMethodID = 11,
                            Description = "",
                            Name = "Full Format",
                            OverwritePass = 1
                        });
                });

            modelBuilder.Entity("EraZor.Models.WipeReport", b =>
                {
                    b.Property<int>("Capacity")
                        .HasColumnType("integer");

                    b.Property<string>("DiskType")
                        .HasColumnType("text");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("OverwritePasses")
                        .HasColumnType("integer");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Status")
                        .HasColumnType("text");

                    b.Property<int?>("WipeJobId")
                        .HasColumnType("integer");

                    b.Property<string>("WipeMethodName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasIndex("WipeJobId");

                    b.ToTable((string)null);

                    b.ToView("WipeReports", (string)null);
                });

            modelBuilder.Entity("WipeJob", b =>
                {
                    b.Property<int>("WipeJobId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("WipeJobId"));

                    b.Property<int>("DiskId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Status")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("WipeMethodId")
                        .HasColumnType("integer");

                    b.HasKey("WipeJobId");

                    b.HasIndex("DiskId");

                    b.HasIndex("WipeMethodId");

                    b.ToTable("WipeJobs");
                });

            modelBuilder.Entity("EraZor.Models.LogEntry", b =>
                {
                    b.HasOne("WipeJob", "WipeJob")
                        .WithMany("LogEntries")
                        .HasForeignKey("WipeJobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WipeJob");
                });

            modelBuilder.Entity("EraZor.Models.WipeReport", b =>
                {
                    b.HasOne("WipeJob", "WipeJob")
                        .WithMany()
                        .HasForeignKey("WipeJobId");

                    b.Navigation("WipeJob");
                });

            modelBuilder.Entity("WipeJob", b =>
                {
                    b.HasOne("EraZor.Models.Disk", "Disk")
                        .WithMany("WipeJobs")
                        .HasForeignKey("DiskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EraZor.Models.WipeMethod", "WipeMethod")
                        .WithMany("WipeJobs")
                        .HasForeignKey("WipeMethodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Disk");

                    b.Navigation("WipeMethod");
                });

            modelBuilder.Entity("EraZor.Models.Disk", b =>
                {
                    b.Navigation("WipeJobs");
                });

            modelBuilder.Entity("EraZor.Models.WipeMethod", b =>
                {
                    b.Navigation("WipeJobs");
                });

            modelBuilder.Entity("WipeJob", b =>
                {
                    b.Navigation("LogEntries");
                });
#pragma warning restore 612, 618
        }
    }
}
