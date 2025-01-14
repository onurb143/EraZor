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
    [Migration("20250113093007_AddedNewFieldsToDisk")]
    partial class AddedNewFieldsToDisk
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("EraZor.Model.Disk", b =>
                {
                    b.Property<int>("DiskID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("DiskID"));

                    b.Property<int>("Capacity")
                        .HasColumnType("integer");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("character varying(24)");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("character varying(8)");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasMaxLength(18)
                        .HasColumnType("character varying(18)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("DiskID");

                    b.HasIndex("SerialNumber")
                        .IsUnique();

                    b.ToTable("Disks");
                });

            modelBuilder.Entity("EraZor.Model.WipeMethod", b =>
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
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<int>("OverwritePass")
                        .HasColumnType("integer");

                    b.HasKey("WipeMethodID");

                    b.ToTable("WipeMethods");

                    b.HasData(
                        new
                        {
                            WipeMethodID = 1,
                            Description = "Sikker metode, der udføres på hardware-niveau via SSD-controlleren. Ideel til SSD'er og NVMe. Ikke ISO-certificeret.",
                            Name = "Secure Erase",
                            OverwritePass = 1
                        },
                        new
                        {
                            WipeMethodID = 2,
                            Description = "Overskriver med nulværdier i ét gennemløb. Velegnet til HDD'er, mindre egnet til SSD'er pga. wear leveling. Ikke ISO-certificeret.",
                            Name = "Zero Fill",
                            OverwritePass = 1
                        },
                        new
                        {
                            WipeMethodID = 3,
                            Description = "Overskriver med tilfældige data i ét gennemløb. Velegnet til HDD'er, mindre egnet til SSD'er. Ikke ISO-certificeret.",
                            Name = "Random Fill",
                            OverwritePass = 1
                        },
                        new
                        {
                            WipeMethodID = 4,
                            Description = "Avanceret metode med 35 gennemløb designet til ældre HDD'er. Ikke egnet til moderne HDD'er, SSD'er eller NVMe. Ikke ISO-certificeret.",
                            Name = "Gutmann Method",
                            OverwritePass = 35
                        },
                        new
                        {
                            WipeMethodID = 5,
                            Description = "Overskriver med tilfældige data i 3 gennemløb. Velegnet til HDD'er, mindre egnet til SSD'er. Ikke ISO-certificeret.",
                            Name = "Random Data",
                            OverwritePass = 3
                        },
                        new
                        {
                            WipeMethodID = 6,
                            Description = "Skriver nulværdier i ét gennemløb. God til HDD'er, mindre effektiv på SSD'er. Ikke ISO-certificeret.",
                            Name = "Write Zero",
                            OverwritePass = 1
                        },
                        new
                        {
                            WipeMethodID = 7,
                            Description = "Metode med 7 gennemløb, som er sikker og velegnet til HDD'er. Overkill for SSD'er og NVMe. Ikke ISO-certificeret.",
                            Name = "Schneier Method",
                            OverwritePass = 7
                        },
                        new
                        {
                            WipeMethodID = 8,
                            Description = "Standardiseret metode med 3 gennemløb. Velegnet til HDD'er. Ikke egnet til SSD'er eller NVMe. Ikke ISO-certificeret.",
                            Name = "HMG IS5 (Enhanced)",
                            OverwritePass = 3
                        },
                        new
                        {
                            WipeMethodID = 9,
                            Description = "Ekstremt sikker metode med 35 gennemløb, designet til ældre HDD'er. Ikke egnet til SSD'er eller NVMe. Ikke ISO-certificeret.",
                            Name = "Peter Gutmann's Method",
                            OverwritePass = 35
                        },
                        new
                        {
                            WipeMethodID = 10,
                            Description = "Hurtig metode med ét gennemløb af nulværdier. Velegnet til HDD'er, men mindre effektiv for SSD'er pga. wear leveling. Ikke ISO-certificeret.",
                            Name = "Single Pass Zeroing",
                            OverwritePass = 1
                        },
                        new
                        {
                            WipeMethodID = 11,
                            Description = "DoD-standard med 4 gennemløb. Velegnet til HDD'er, mindre relevant for SSD'er. Ikke ISO-certificeret.",
                            Name = "DoD 5220.22-M (E)",
                            OverwritePass = 4
                        },
                        new
                        {
                            WipeMethodID = 12,
                            Description = "ISO-standardiseret metode med ét gennemløb af nulværdier. Ideel til SSD'er, NVMe og HDD'er. ISO-certificeret.",
                            Name = "ISO/IEC 27040",
                            OverwritePass = 1
                        });
                });

            modelBuilder.Entity("EraZor.Model.WipeReport", b =>
                {
                    b.Property<int>("Capacity")
                        .HasColumnType("integer");

                    b.Property<string>("DiskType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Manufacturer")
                        .IsRequired()
                        .HasMaxLength(24)
                        .HasColumnType("character varying(24)");

                    b.Property<int>("OverwritePasses")
                        .HasColumnType("integer");

                    b.Property<string>("PerformedBy")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("SerialNumber")
                        .IsRequired()
                        .HasMaxLength(18)
                        .HasColumnType("character varying(18)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("WipeJobId")
                        .HasColumnType("integer");

                    b.Property<string>("WipeMethodName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasIndex("UserId");

                    b.HasIndex("WipeJobId");

                    b.ToTable((string)null);

                    b.ToView("WipeReports", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

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
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
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

                    b.Property<string>("PerformedByUserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("WipeMethodId")
                        .HasColumnType("integer");

                    b.HasKey("WipeJobId");

                    b.HasIndex("DiskId");

                    b.HasIndex("PerformedByUserId");

                    b.HasIndex("WipeMethodId");

                    b.ToTable("WipeJobs");
                });

            modelBuilder.Entity("EraZor.Model.WipeReport", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WipeJob", "WipeJob")
                        .WithMany()
                        .HasForeignKey("WipeJobId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");

                    b.Navigation("WipeJob");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WipeJob", b =>
                {
                    b.HasOne("EraZor.Model.Disk", "Disk")
                        .WithMany("WipeJobs")
                        .HasForeignKey("DiskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", "PerformedByUser")
                        .WithMany()
                        .HasForeignKey("PerformedByUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("EraZor.Model.WipeMethod", "WipeMethod")
                        .WithMany("WipeJobs")
                        .HasForeignKey("WipeMethodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Disk");

                    b.Navigation("PerformedByUser");

                    b.Navigation("WipeMethod");
                });

            modelBuilder.Entity("EraZor.Model.Disk", b =>
                {
                    b.Navigation("WipeJobs");
                });

            modelBuilder.Entity("EraZor.Model.WipeMethod", b =>
                {
                    b.Navigation("WipeJobs");
                });
#pragma warning restore 612, 618
        }
    }
}
