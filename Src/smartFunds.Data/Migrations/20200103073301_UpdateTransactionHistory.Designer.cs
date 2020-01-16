﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using smartFunds.Data;

namespace smartFunds.Data.Migrations
{
    [DbContext(typeof(smartFundsDbContext))]
    [Migration("20200103073301_UpdateTransactionHistory")]
    partial class UpdateTransactionHistory
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
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
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("smartFunds.Data.Models.AdminTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FundId");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<DateTime>("LastUpdatedDate");

                    b.Property<int>("Status");

                    b.Property<decimal>("TransactionAmount");

                    b.Property<int>("TransactionType");

                    b.HasKey("Id");

                    b.HasIndex("FundId");

                    b.ToTable("Tasks");
                });

            modelBuilder.Entity("smartFunds.Data.Models.ContactCMS", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email");

                    b.Property<string>("EmailForReceiving");

                    b.Property<string>("Phone");

                    b.HasKey("Id");

                    b.ToTable("ContactConfigurations");
                });

            modelBuilder.Entity("smartFunds.Data.Models.CustomerLevel", b =>
                {
                    b.Property<int>("IDCustomerLevel")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("CertificateValue");

                    b.Property<DateTime>("DateLastUpdated");

                    b.Property<string>("DeletedAt");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<decimal>("MaxMoney");

                    b.Property<decimal>("MinMoney");

                    b.Property<decimal>("NAV");

                    b.Property<string>("NameCustomerLevel");

                    b.HasKey("IDCustomerLevel");

                    b.ToTable("CustomerLevel");
                });

            modelBuilder.Entity("smartFunds.Data.Models.FAQ", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Category");

                    b.Property<string>("Content");

                    b.Property<DateTime>("DateLastUpdated");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("FAQs");
                });

            modelBuilder.Entity("smartFunds.Data.Models.Fund", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Code");

                    b.Property<string>("Content");

                    b.Property<DateTime>("DateLastApproved");

                    b.Property<DateTime>("DateLastUpdated");

                    b.Property<string>("DeletedAt");

                    b.Property<int>("EditStatus");

                    b.Property<bool>("IsBalancing");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Link");

                    b.Property<decimal>("NAV");

                    b.Property<decimal>("NAVNew");

                    b.Property<decimal>("NAVOld");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Fund");
                });

            modelBuilder.Entity("smartFunds.Data.Models.FundPurchaseFee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Fee");

                    b.Property<decimal>("From");

                    b.Property<int>("FromLabel");

                    b.Property<int>("FundId");

                    b.Property<decimal>("To");

                    b.Property<int>("ToLabel");

                    b.HasKey("Id");

                    b.HasIndex("FundId");

                    b.ToTable("FundPurchaseFees");
                });

            modelBuilder.Entity("smartFunds.Data.Models.FundSellFee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Fee");

                    b.Property<int>("From");

                    b.Property<int>("FromLabel");

                    b.Property<int>("FundId");

                    b.Property<int>("To");

                    b.Property<int>("ToLabel");

                    b.HasKey("Id");

                    b.HasIndex("FundId");

                    b.ToTable("FundSellFees");
                });

            modelBuilder.Entity("smartFunds.Data.Models.FundTransactionHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("FundId");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<DateTime>("LastUpdatedDate");

                    b.Property<decimal>("NoOfCertificates");

                    b.Property<int>("Status");

                    b.Property<decimal>("TotalInvestNoOfCertificates");

                    b.Property<decimal>("TotalWithdrawnNoOfCertificates");

                    b.Property<DateTime>("TransactionDate");

                    b.Property<int>("TransactionType");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("FundId");

                    b.HasIndex("UserId");

                    b.ToTable("FundTransactionHistory");
                });

            modelBuilder.Entity("smartFunds.Data.Models.GenericIntroducingSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Banner");

                    b.Property<string>("Description");

                    b.Property<string>("MobileBanner");

                    b.HasKey("Id");

                    b.ToTable("GenericIntroducingSettings");
                });

            modelBuilder.Entity("smartFunds.Data.Models.HangFire.Job", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime?>("ExpireAt");

                    b.Property<string>("StateName");

                    b.HasKey("Id");

                    b.ToTable("Job","HangFire");
                });

            modelBuilder.Entity("smartFunds.Data.Models.HomepageCMS", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Category");

                    b.Property<string>("ImageName");

                    b.HasKey("Id");

                    b.ToTable("HomepageConfigurations");
                });

            modelBuilder.Entity("smartFunds.Data.Models.Investment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount");

                    b.Property<DateTime>("DateInvestment");

                    b.Property<decimal>("RemainAmount");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Investment");
                });

            modelBuilder.Entity("smartFunds.Data.Models.InvestmentTarget", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateLastUpdated");

                    b.Property<string>("DeletedAt");

                    b.Property<int>("Duration");

                    b.Property<decimal>("ExpectedAmount");

                    b.Property<int>("Frequency");

                    b.Property<int>("InvestmentMethod");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<decimal>("OneTimeAmount");

                    b.Property<int>("Status");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("InvestmentTargets");
                });

            modelBuilder.Entity("smartFunds.Data.Models.InvestmentTargetSetting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Duration");

                    b.Property<int>("PortfolioId");

                    b.Property<decimal>("Value");

                    b.HasKey("Id");

                    b.HasIndex("PortfolioId");

                    b.ToTable("InvestmentTargetSettings");
                });

            modelBuilder.Entity("smartFunds.Data.Models.KVRR", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateLastUpdated");

                    b.Property<string>("DeletedAt")
                        .HasMaxLength(30);

                    b.Property<string>("Detail");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("KVRRImagePath");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Kvrr");
                });

            modelBuilder.Entity("smartFunds.Data.Models.KVRRAnswer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<int?>("KVRRQuestionId");

                    b.Property<int?>("Mark");

                    b.HasKey("Id");

                    b.HasIndex("KVRRQuestionId");

                    b.ToTable("KvrrAnswer");
                });

            modelBuilder.Entity("smartFunds.Data.Models.KVRRMark", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateLastUpdated");

                    b.Property<int?>("KVRRId");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int>("MarkFrom");

                    b.Property<int>("MarkTo");

                    b.HasKey("Id");

                    b.HasIndex("KVRRId");

                    b.ToTable("KvrrMark");
                });

            modelBuilder.Entity("smartFunds.Data.Models.KVRRPortfolio", b =>
                {
                    b.Property<int>("KVRRId");

                    b.Property<int>("PortfolioId");

                    b.HasKey("KVRRId", "PortfolioId");

                    b.HasIndex("PortfolioId");

                    b.ToTable("KVRRPortfolio");
                });

            modelBuilder.Entity("smartFunds.Data.Models.KVRRQuestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<string>("ImageDesktop");

                    b.Property<string>("ImageMobile");

                    b.Property<int>("KVRRQuestionCategories");

                    b.Property<int>("No");

                    b.HasKey("Id");

                    b.ToTable("KvrrQuestion");
                });

            modelBuilder.Entity("smartFunds.Data.Models.MaintainingFee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("AmountFrom");

                    b.Property<decimal>("AmountTo");

                    b.Property<decimal>("Percentage")
                        .HasColumnType("decimal(12,4)");

                    b.HasKey("Id");

                    b.ToTable("MaintainingFees");
                });

            modelBuilder.Entity("smartFunds.Data.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CheckSum");

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Desc");

                    b.Property<bool>("IsInvestmentTarget");

                    b.Property<bool>("IsSuccess");

                    b.Property<string>("MerchantCode");

                    b.Property<string>("Msisdn");

                    b.Property<string>("TransAmount");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime>("UpdatedDate");

                    b.Property<string>("Version");

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("smartFunds.Data.Models.OrderRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CreatedBy");

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("ErrorCode");

                    b.Property<string>("FullName");

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("UpdatedBy");

                    b.Property<DateTime>("UpdatedDate");

                    b.HasKey("Id");

                    b.ToTable("OrderRequests");
                });

            modelBuilder.Entity("smartFunds.Data.Models.Portfolio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content");

                    b.Property<DateTime>("DateLastUpdated");

                    b.Property<string>("DeletedAt");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("Portfolio");
                });

            modelBuilder.Entity("smartFunds.Data.Models.PortfolioFund", b =>
                {
                    b.Property<int>("PortfolioId");

                    b.Property<int>("FundId");

                    b.Property<DateTime>("DateLastUpdated");

                    b.Property<int>("EditStatus");

                    b.Property<decimal?>("FundPercent");

                    b.Property<decimal?>("FundPercentNew");

                    b.Property<string>("LastUpdatedBy");

                    b.HasKey("PortfolioId", "FundId");

                    b.HasIndex("FundId");

                    b.ToTable("PortfolioFund");
                });

            modelBuilder.Entity("smartFunds.Data.Models.TaskCompleted", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("LastUpdatedBy");

                    b.Property<DateTime>("LastUpdatedDate");

                    b.Property<int>("ObjectID");

                    b.Property<string>("ObjectName");

                    b.Property<int>("TaskType");

                    b.Property<decimal>("TransactionAmount");

                    b.HasKey("Id");

                    b.ToTable("TasksCompleted");
                });

            modelBuilder.Entity("smartFunds.Data.Models.Test", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Tests");
                });

            modelBuilder.Entity("smartFunds.Data.Models.TransactionHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Amount");

                    b.Property<decimal>("CurrentAccountAmount");

                    b.Property<string>("Description");

                    b.Property<int>("ObjectId");

                    b.Property<int?>("RemittanceStatus");

                    b.Property<int>("Status");

                    b.Property<decimal>("TotalWithdrawal");

                    b.Property<DateTime>("TransactionDate");

                    b.Property<int>("TransactionType");

                    b.Property<string>("UserId");

                    b.Property<int?>("WithdrawalType");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("TransactionHistory");
                });

            modelBuilder.Entity("smartFunds.Data.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<decimal>("AdjustmentFactor")
                        .HasColumnType("decimal(18,12)");

                    b.Property<decimal>("AmountWithdrawn");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("Created");

                    b.Property<decimal>("CurrentAccountAmount");

                    b.Property<decimal>("CurrentAmountWithdrawn");

                    b.Property<decimal>("CurrentInvestmentAmount")
                        .HasColumnType("decimal(18,5)");

                    b.Property<DateTime>("DateLastUpdated");

                    b.Property<string>("DeletedAt");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FullName");

                    b.Property<decimal>("InitialInvestmentAmount");

                    b.Property<bool>("IsDeleted");

                    b.Property<int?>("KVRRId");

                    b.Property<DateTime>("LastLogin");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<bool>("WithdrawProcessing");

                    b.Property<DateTime>("WithdrawProcessingDate");

                    b.HasKey("Id");

                    b.HasIndex("KVRRId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("smartFunds.Data.Models.UserFund", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<int>("FundId");

                    b.Property<int>("EditStatus");

                    b.Property<decimal?>("NoOfCertificates");

                    b.HasKey("UserId", "FundId");

                    b.HasIndex("FundId");

                    b.ToTable("UserFunds");
                });

            modelBuilder.Entity("smartFunds.Data.Models.WithdrawalFee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Percentage")
                        .HasColumnType("decimal(12,4)");

                    b.Property<int>("TimeInvestmentBegin");

                    b.Property<int>("TimeInvestmentEnd");

                    b.HasKey("Id");

                    b.ToTable("WithdrawalFees");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("smartFunds.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("smartFunds.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("smartFunds.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("smartFunds.Data.Models.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("smartFunds.Data.Models.AdminTask", b =>
                {
                    b.HasOne("smartFunds.Data.Models.Fund", "Fund")
                        .WithMany()
                        .HasForeignKey("FundId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("smartFunds.Data.Models.FundPurchaseFee", b =>
                {
                    b.HasOne("smartFunds.Data.Models.Fund", "Fund")
                        .WithMany()
                        .HasForeignKey("FundId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("smartFunds.Data.Models.FundSellFee", b =>
                {
                    b.HasOne("smartFunds.Data.Models.Fund", "Fund")
                        .WithMany()
                        .HasForeignKey("FundId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("smartFunds.Data.Models.FundTransactionHistory", b =>
                {
                    b.HasOne("smartFunds.Data.Models.Fund", "Fund")
                        .WithMany()
                        .HasForeignKey("FundId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("smartFunds.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("smartFunds.Data.Models.Investment", b =>
                {
                    b.HasOne("smartFunds.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("smartFunds.Data.Models.InvestmentTarget", b =>
                {
                    b.HasOne("smartFunds.Data.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("smartFunds.Data.Models.InvestmentTargetSetting", b =>
                {
                    b.HasOne("smartFunds.Data.Models.Portfolio", "Portfolio")
                        .WithMany()
                        .HasForeignKey("PortfolioId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("smartFunds.Data.Models.KVRRAnswer", b =>
                {
                    b.HasOne("smartFunds.Data.Models.KVRRQuestion", "KVRRQuestion")
                        .WithMany("KVRRAnswers")
                        .HasForeignKey("KVRRQuestionId");
                });

            modelBuilder.Entity("smartFunds.Data.Models.KVRRMark", b =>
                {
                    b.HasOne("smartFunds.Data.Models.KVRR", "KVRR")
                        .WithMany()
                        .HasForeignKey("KVRRId");
                });

            modelBuilder.Entity("smartFunds.Data.Models.KVRRPortfolio", b =>
                {
                    b.HasOne("smartFunds.Data.Models.KVRR", "KVRR")
                        .WithMany("KVRRPortfolios")
                        .HasForeignKey("KVRRId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("smartFunds.Data.Models.Portfolio", "Portfolio")
                        .WithMany("KVRRPortfolios")
                        .HasForeignKey("PortfolioId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("smartFunds.Data.Models.PortfolioFund", b =>
                {
                    b.HasOne("smartFunds.Data.Models.Fund", "Fund")
                        .WithMany("PortfolioFunds")
                        .HasForeignKey("FundId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("smartFunds.Data.Models.Portfolio", "Portfolio")
                        .WithMany("PortfolioFunds")
                        .HasForeignKey("PortfolioId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("smartFunds.Data.Models.TransactionHistory", b =>
                {
                    b.HasOne("smartFunds.Data.Models.User", "User")
                        .WithMany("TransactionHistory")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("smartFunds.Data.Models.User", b =>
                {
                    b.HasOne("smartFunds.Data.Models.KVRR", "KVRR")
                        .WithMany()
                        .HasForeignKey("KVRRId");
                });

            modelBuilder.Entity("smartFunds.Data.Models.UserFund", b =>
                {
                    b.HasOne("smartFunds.Data.Models.Fund", "Fund")
                        .WithMany("UserFunds")
                        .HasForeignKey("FundId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("smartFunds.Data.Models.User", "User")
                        .WithMany("UserFunds")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
