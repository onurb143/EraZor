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
    [Migration("20241214153643_FixWipeReport")]
    partial class FixWipeReport
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

            modelBuilder.Entity("EraZor.Models.WipeReport", b =>
                {
                    b.Property<int?>("WipeJobId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

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

                    b.Property<string>("WipeMethodName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("WipeJobId");

                    b.ToTable("WipeReports");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("text");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
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
                        .WithMany("WipeReports")
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

                    b.Navigation("WipeReports");
                });
#pragma warning restore 612, 618
        }
    }
}