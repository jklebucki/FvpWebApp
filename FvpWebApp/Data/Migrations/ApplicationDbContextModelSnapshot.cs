﻿// <auto-generated />
using System;
using FvpWebApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FvpWebApp.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("FvpWebAppModels.Models.Contractor", b =>
                {
                    b.Property<int>("ContractorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("CheckDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ContractorErpId")
                        .HasColumnType("int");

                    b.Property<int>("ContractorSourceId")
                        .HasColumnType("int");

                    b.Property<bool>("ContractorValid")
                        .HasColumnType("bit");

                    b.Property<string>("CountryCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SourceId")
                        .HasColumnType("int");

                    b.Property<string>("StreetAndNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VatId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ContractorId");

                    b.HasIndex("SourceId");

                    b.ToTable("Contractors");
                });

            modelBuilder.Entity("FvpWebAppModels.Models.Document", b =>
                {
                    b.Property<int>("DocumentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ContractorId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("DocContractorCity")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocContractorCountryCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocContractorId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocContractorName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocContractorPostCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocContractorStreetAndNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocContractorVatCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DocumentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DocumentNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocumentSymbol")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("DocumentValid")
                        .HasColumnType("bit");

                    b.Property<int>("ExternalId")
                        .HasColumnType("int");

                    b.Property<decimal>("Gross")
                        .HasColumnType("decimal(12,4)");

                    b.Property<decimal>("Net")
                        .HasColumnType("decimal(12,4)");

                    b.Property<DateTime>("SaleDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("SourceId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Vat")
                        .HasColumnType("decimal(12,4)");

                    b.HasKey("DocumentId");

                    b.HasIndex("ContractorId");

                    b.HasIndex("SourceId");

                    b.ToTable("Documents");
                });

            modelBuilder.Entity("FvpWebAppModels.Models.DocumentVat", b =>
                {
                    b.Property<int>("DocumentVatId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("DocumentId")
                        .HasColumnType("int");

                    b.Property<decimal>("GrossAmount")
                        .HasColumnType("decimal(12,4)");

                    b.Property<decimal>("NetAmount")
                        .HasColumnType("decimal(12,4)");

                    b.Property<decimal>("VatAmount")
                        .HasColumnType("decimal(12,4)");

                    b.Property<string>("VatCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VatTags")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("VatValue")
                        .HasColumnType("decimal(4,2)");

                    b.HasKey("DocumentVatId");

                    b.HasIndex("DocumentId");

                    b.ToTable("DocumentVats");
                });

            modelBuilder.Entity("FvpWebAppModels.Models.Source", b =>
                {
                    b.Property<int>("SourceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DbName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("TargetId")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SourceId");

                    b.HasIndex("TargetId");

                    b.ToTable("Sources");

                    b.HasData(
                        new
                        {
                            SourceId = 1,
                            Address = "192.168.42.70",
                            Code = "DP1",
                            DbName = "sben",
                            Description = "Dyskont Paliwowy Słowiańska",
                            Password = "almarwinnet",
                            Type = "oracle_sben_dp",
                            Username = "sben"
                        },
                        new
                        {
                            SourceId = 2,
                            Address = "192.168.45.70",
                            Code = "DP2",
                            DbName = "sben",
                            Description = "Dyskont Paliwowy Słowiańska",
                            Password = "almarwinnet",
                            Type = "oracle_sben_dp",
                            Username = "sben"
                        },
                        new
                        {
                            SourceId = 3,
                            Address = "I:\\DaneBP\\MOSTKI",
                            Code = "BPMOSTKI",
                            DbName = "",
                            Description = "Stacja Paliw BP Mostki",
                            Password = "",
                            Type = "bp_flat_file",
                            Username = ""
                        });
                });

            modelBuilder.Entity("FvpWebAppModels.Models.SourceType", b =>
                {
                    b.Property<int>("SourceTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Descryption")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("SourceTypeId");

                    b.ToTable("SourceTypes");
                });

            modelBuilder.Entity("FvpWebAppModels.Models.Target", b =>
                {
                    b.Property<int>("TargetId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("DatabaseAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DatabaseName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DatabasePassword")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DatabaseUsername")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Descryption")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TargetId");

                    b.ToTable("Targets");

                    b.HasData(
                        new
                        {
                            TargetId = 1,
                            DatabaseAddress = "192.168.21.20",
                            DatabaseName = "CITRONEX_FK",
                            DatabasePassword = "#sa2015!",
                            DatabaseUsername = "sa",
                            Descryption = "Citronex I - Symfonia ERP"
                        },
                        new
                        {
                            TargetId = 2,
                            DatabaseAddress = "192.168.21.20",
                            DatabaseName = "CITRONEX_MOP",
                            DatabasePassword = "#sa2015!",
                            DatabaseUsername = "sa",
                            Descryption = "Citronex MOP - Symfonia ERP"
                        });
                });

            modelBuilder.Entity("FvpWebAppModels.Models.TaskTicket", b =>
                {
                    b.Property<int>("TaskTicketId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateTo")
                        .HasColumnType("datetime2");

                    b.Property<int>("SourceId")
                        .HasColumnType("int");

                    b.Property<int>("TikcketStatus")
                        .HasColumnType("int");

                    b.HasKey("TaskTicketId");

                    b.ToTable("TaskTickets");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("FvpWebAppModels.Models.Contractor", b =>
                {
                    b.HasOne("FvpWebAppModels.Models.Source", null)
                        .WithMany("Contractors")
                        .HasForeignKey("SourceId");
                });

            modelBuilder.Entity("FvpWebAppModels.Models.Document", b =>
                {
                    b.HasOne("FvpWebAppModels.Models.Contractor", null)
                        .WithMany("Documents")
                        .HasForeignKey("ContractorId");

                    b.HasOne("FvpWebAppModels.Models.Source", null)
                        .WithMany("Documents")
                        .HasForeignKey("SourceId");
                });

            modelBuilder.Entity("FvpWebAppModels.Models.DocumentVat", b =>
                {
                    b.HasOne("FvpWebAppModels.Models.Document", null)
                        .WithMany("DocumentVats")
                        .HasForeignKey("DocumentId");
                });

            modelBuilder.Entity("FvpWebAppModels.Models.Source", b =>
                {
                    b.HasOne("FvpWebAppModels.Models.Target", null)
                        .WithMany("Sources")
                        .HasForeignKey("TargetId");
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
#pragma warning restore 612, 618
        }
    }
}
