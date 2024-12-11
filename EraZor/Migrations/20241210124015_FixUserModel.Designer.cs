﻿// <auto-generated />
using System;
using EraZor.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EraZor.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20241210124015_FixUserModel")]
    partial class FixUserModel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.2")
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
                        .HasColumnType("text");

                    b.Property<string>("Path")
                        .HasColumnType("text");

                    b.Property<string>("SerialNumber")
                        .HasColumnType("text");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("DiskID");

                    b.ToTable("Disks");
                });

            modelBuilder.Entity("EraZor.Models.LogEntry", b =>
                {
                    b.Property<int>("LogID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("LogID"));

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("WipeJobId")
                        .HasColumnType("integer");

                    b.HasKey("LogID");

                    b.HasIndex("WipeJobId");

                    b.ToTable("LogEntries");
                });

            modelBuilder.Entity("EraZor.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserId"));

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EraZor.Models.WipeMethod", b =>
                {
                    b.Property<int>("WipeMethodID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("WipeMethodID"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("OverwritePass")
                        .HasColumnType("integer");

                    b.HasKey("WipeMethodID");

                    b.ToTable("WipeMethods");
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
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("WipeMethodId")
                        .HasColumnType("integer");

                    b.HasKey("WipeJobId");

                    b.HasIndex("DiskId");

                    b.HasIndex("UserId");

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

            modelBuilder.Entity("WipeJob", b =>
                {
                    b.HasOne("EraZor.Models.Disk", "Disk")
                        .WithMany("WipeJobs")
                        .HasForeignKey("DiskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EraZor.Models.User", "User")
                        .WithMany("WipeJobs")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EraZor.Models.WipeMethod", "WipeMethod")
                        .WithMany("WipeJobs")
                        .HasForeignKey("WipeMethodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Disk");

                    b.Navigation("User");

                    b.Navigation("WipeMethod");
                });

            modelBuilder.Entity("EraZor.Models.Disk", b =>
                {
                    b.Navigation("WipeJobs");
                });

            modelBuilder.Entity("EraZor.Models.User", b =>
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
