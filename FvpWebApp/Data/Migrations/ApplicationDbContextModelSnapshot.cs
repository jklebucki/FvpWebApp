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

                    b.Property<int?>("ContractorErpPosition")
                        .HasColumnType("int");

                    b.Property<string>("ContractorSourceId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ContractorStatus")
                        .HasColumnType("int");

                    b.Property<string>("CountryCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EstateNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Firm")
                        .HasColumnType("int");

                    b.Property<int?>("GusContractorEntriesCount")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Province")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("QuartersNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Regon")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SourceId")
                        .HasColumnType("int");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VatId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ContractorId");

                    b.HasIndex("SourceId");

                    b.ToTable("Contractors");
                });

            modelBuilder.Entity("FvpWebAppModels.Models.Country", b =>
                {
                    b.Property<int>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Info")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameENG")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NamePL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Symbol")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("UE")
                        .HasColumnType("bit");

                    b.HasKey("CountryId");

                    b.ToTable("Countries");

                    b.HasData(
                        new
                        {
                            CountryId = 1,
                            NamePL = "Austria",
                            Symbol = "AT",
                            UE = true
                        },
                        new
                        {
                            CountryId = 2,
                            NamePL = "Belgia",
                            Symbol = "BE",
                            UE = true
                        },
                        new
                        {
                            CountryId = 3,
                            NamePL = "Bułgaria",
                            Symbol = "BG",
                            UE = true
                        },
                        new
                        {
                            CountryId = 4,
                            NamePL = "Cypr",
                            Symbol = "CY",
                            UE = true
                        },
                        new
                        {
                            CountryId = 5,
                            NamePL = "Czechy",
                            Symbol = "CZ",
                            UE = true
                        },
                        new
                        {
                            CountryId = 6,
                            NamePL = "Niemcy",
                            Symbol = "DE",
                            UE = true
                        },
                        new
                        {
                            CountryId = 7,
                            NamePL = "Dania",
                            Symbol = "DK",
                            UE = true
                        },
                        new
                        {
                            CountryId = 8,
                            NamePL = "Estonia",
                            Symbol = "EE",
                            UE = true
                        },
                        new
                        {
                            CountryId = 9,
                            NamePL = "Grecja",
                            Symbol = "EL",
                            UE = true
                        },
                        new
                        {
                            CountryId = 10,
                            NamePL = "Hiszpania",
                            Symbol = "ES",
                            UE = true
                        },
                        new
                        {
                            CountryId = 11,
                            NamePL = "Finlandia",
                            Symbol = "FI",
                            UE = true
                        },
                        new
                        {
                            CountryId = 12,
                            NamePL = "Francja",
                            Symbol = "FR",
                            UE = true
                        },
                        new
                        {
                            CountryId = 13,
                            NamePL = "Wielka Brytania",
                            Symbol = "GB",
                            UE = true
                        },
                        new
                        {
                            CountryId = 14,
                            NamePL = "Chorwacja",
                            Symbol = "HR",
                            UE = true
                        },
                        new
                        {
                            CountryId = 15,
                            NamePL = "Węgry",
                            Symbol = "HU",
                            UE = true
                        },
                        new
                        {
                            CountryId = 16,
                            NamePL = "Irlandia",
                            Symbol = "IE",
                            UE = true
                        },
                        new
                        {
                            CountryId = 17,
                            NamePL = "Włochy",
                            Symbol = "IT",
                            UE = true
                        },
                        new
                        {
                            CountryId = 18,
                            NamePL = "Litwa",
                            Symbol = "LT",
                            UE = true
                        },
                        new
                        {
                            CountryId = 19,
                            NamePL = "Luksemburg",
                            Symbol = "LU",
                            UE = true
                        },
                        new
                        {
                            CountryId = 20,
                            NamePL = "Łotwa",
                            Symbol = "LV",
                            UE = true
                        },
                        new
                        {
                            CountryId = 21,
                            NamePL = "Malta",
                            Symbol = "MT",
                            UE = true
                        },
                        new
                        {
                            CountryId = 22,
                            NamePL = "Holandia",
                            Symbol = "NL",
                            UE = true
                        },
                        new
                        {
                            CountryId = 23,
                            NamePL = "Polska",
                            Symbol = "PL",
                            UE = true
                        },
                        new
                        {
                            CountryId = 24,
                            NamePL = "Portugalia",
                            Symbol = "PT",
                            UE = true
                        },
                        new
                        {
                            CountryId = 25,
                            NamePL = "Rumunia",
                            Symbol = "RO",
                            UE = true
                        },
                        new
                        {
                            CountryId = 26,
                            NamePL = "Szwecja",
                            Symbol = "SE",
                            UE = true
                        },
                        new
                        {
                            CountryId = 27,
                            NamePL = "Słowenia",
                            Symbol = "SI",
                            UE = true
                        },
                        new
                        {
                            CountryId = 28,
                            NamePL = "Słowacja",
                            Symbol = "SK",
                            UE = true
                        });
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

                    b.Property<int>("DocContractorFirm")
                        .HasColumnType("int");

                    b.Property<string>("DocContractorId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocContractorName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocContractorPostCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocContractorStreetAndNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocContractorVatId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DocumentDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DocumentNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("DocumentStatus")
                        .HasColumnType("int");

                    b.Property<string>("DocumentSymbol")
                        .HasColumnType("nvarchar(max)");

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

                    b.Property<int>("TaskTicketId")
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

                    b.HasIndex("TaskTicketId");

                    b.HasIndex("DocumentNumber", "SourceId")
                        .IsUnique()
                        .HasName("IX_DocumentUniqueInSource")
                        .HasFilter("[DocumentNumber] IS NOT NULL AND [SourceId] IS NOT NULL");

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
                            TargetId = 1,
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
                            TargetId = 1,
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
                            TargetId = 1,
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

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateFrom")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateTo")
                        .HasColumnType("datetime2");

                    b.Property<int>("SourceId")
                        .HasColumnType("int");

                    b.Property<DateTime>("StatusChangedAt")
                        .HasColumnType("datetime2");

                    b.Property<int>("TicketStatus")
                        .HasColumnType("int");

                    b.Property<int>("TicketType")
                        .HasColumnType("int");

                    b.HasKey("TaskTicketId");

                    b.ToTable("TaskTickets");

                    b.HasData(
                        new
                        {
                            TaskTicketId = 1,
                            CreatedAt = new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateFrom = new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateTo = new DateTime(2020, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SourceId = 1,
                            StatusChangedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TicketStatus = 0,
                            TicketType = 0
                        },
                        new
                        {
                            TaskTicketId = 2,
                            CreatedAt = new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateFrom = new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateTo = new DateTime(2020, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SourceId = 2,
                            StatusChangedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TicketStatus = 0,
                            TicketType = 0
                        },
                        new
                        {
                            TaskTicketId = 3,
                            CreatedAt = new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateFrom = new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateTo = new DateTime(2020, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SourceId = 0,
                            StatusChangedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TicketStatus = 0,
                            TicketType = 2
                        },
                        new
                        {
                            TaskTicketId = 4,
                            CreatedAt = new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateFrom = new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateTo = new DateTime(2020, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SourceId = 1,
                            StatusChangedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TicketStatus = 0,
                            TicketType = 3
                        },
                        new
                        {
                            TaskTicketId = 5,
                            CreatedAt = new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateFrom = new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateTo = new DateTime(2020, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SourceId = 2,
                            StatusChangedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TicketStatus = 0,
                            TicketType = 3
                        },
                        new
                        {
                            TaskTicketId = 6,
                            CreatedAt = new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateFrom = new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateTo = new DateTime(2020, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SourceId = 1,
                            StatusChangedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TicketStatus = 0,
                            TicketType = 4
                        },
                        new
                        {
                            TaskTicketId = 7,
                            CreatedAt = new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateFrom = new DateTime(2020, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DateTo = new DateTime(2020, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            SourceId = 2,
                            StatusChangedAt = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            TicketStatus = 0,
                            TicketType = 4
                        });
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

                    b.HasOne("FvpWebAppModels.Models.TaskTicket", null)
                        .WithMany("Documents")
                        .HasForeignKey("TaskTicketId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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
