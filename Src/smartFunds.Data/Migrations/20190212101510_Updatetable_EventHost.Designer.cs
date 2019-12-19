﻿// <auto-generated />
using System;
using smartFunds.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace smartFunds.Data.Migrations
{
    [DbContext(typeof(smartFundsDbContext))]
    [Migration("20190212101510_Updatetable_EventHost")]
    partial class Updatetable_EventHost
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("smartFunds.Data.Models.Contactbase.Country", b =>
                {
                    b.Property<string>("Code")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Address1Label");

                    b.Property<bool>("Address1Required");

                    b.Property<bool>("Address1Visible");

                    b.Property<string>("Address2Label");

                    b.Property<bool>("Address2Required");

                    b.Property<bool>("Address2Visible");

                    b.Property<string>("Address3Label");

                    b.Property<bool>("Address3Required");

                    b.Property<bool>("Address3Visible");

                    b.Property<string>("Address4Label");

                    b.Property<bool>("Address4Required");

                    b.Property<bool>("Address4Visible");

                    b.Property<string>("Address5Label");

                    b.Property<bool>("Address5Required");

                    b.Property<bool>("Address5Visible");

                    b.Property<string>("Address6Label");

                    b.Property<bool>("Address6Required");

                    b.Property<bool>("Address6Visible");

                    b.Property<string>("Address7Label");

                    b.Property<bool>("Address7Required");

                    b.Property<bool>("Address7Visible");

                    b.Property<string>("Address8Label");

                    b.Property<bool>("Address8Required");

                    b.Property<bool>("Address8Visible");

                    b.Property<string>("AddressFormat");

                    b.Property<string>("DialingPrefix");

                    b.Property<string>("Name");

                    b.Property<int>("RegionId");

                    b.HasKey("Code");

                    b.HasIndex("RegionId");

                    b.ToTable("CountryView","dbo");
                });

            modelBuilder.Entity("smartFunds.Data.Models.Contactbase.Locality", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("CountryCode");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("CountryCode");

                    b.ToTable("LocalityView","dbo");
                });

            modelBuilder.Entity("smartFunds.Data.Models.Contactbase.Member", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Address1");

                    b.Property<string>("Address2");

                    b.Property<string>("Address3");

                    b.Property<string>("Address4");

                    b.Property<string>("Address5");

                    b.Property<string>("Address6");

                    b.Property<string>("Address7");

                    b.Property<string>("Address8");

                    b.Property<string>("AddressFormat");

                    b.Property<int?>("Age");

                    b.Property<string>("CountryCode");

                    b.Property<string>("CountryName");

                    b.Property<DateTime?>("DateOfBirth");

                    b.Property<DateTime?>("DeceasedDate");

                    b.Property<string>("DeceasedSpouse");

                    b.Property<string>("Email");

                    b.Property<bool?>("FatherDeceased");

                    b.Property<int?>("FatherId");

                    b.Property<string>("FathersLocality");

                    b.Property<string>("FathersName");

                    b.Property<string>("FirstName");

                    b.Property<string>("FirstName_SC");

                    b.Property<string>("FullName");

                    b.Property<string>("FullNameReverse");

                    b.Property<string>("Generation");

                    b.Property<string>("HomePhone");

                    b.Property<string>("HomePhoneCode");

                    b.Property<DateTime?>("HomePhoneLastUpdated");

                    b.Property<string>("HouseholdCoupleName");

                    b.Property<string>("HouseholdLastName");

                    b.Property<string>("HouseholdName");

                    b.Property<string>("HouseholdNameDisplay");

                    b.Property<string>("Household_FirstName");

                    b.Property<string>("Household_LastName");

                    b.Property<int?>("HouseholderId");

                    b.Property<string>("HouseholderName");

                    b.Property<bool>("IsDeceased");

                    b.Property<bool>("IsHidden");

                    b.Property<bool>("IsHouseholder");

                    b.Property<string>("LastName");

                    b.Property<string>("LastName_SC");

                    b.Property<int?>("LocalityId");

                    b.Property<string>("LocalityName");

                    b.Property<string>("MaidenName");

                    b.Property<string>("MobilePhone");

                    b.Property<string>("MobilePhoneCode");

                    b.Property<DateTime?>("MobilePhoneLastUpdated");

                    b.Property<bool?>("MotherDeceased");

                    b.Property<int?>("MotherId");

                    b.Property<string>("MothersLocality");

                    b.Property<string>("MothersName");

                    b.Property<string>("PhotoTagCdnPath");

                    b.Property<int?>("RegionId");

                    b.Property<string>("RegionName");

                    b.Property<int?>("SpouseId");

                    b.Property<int?>("SublocalityId");

                    b.Property<bool?>("SublocalityIsMainHall");

                    b.Property<string>("SublocalityName");

                    b.Property<string>("SublocalityShortName");

                    b.Property<string>("Title");

                    b.Property<string>("WorkPhone");

                    b.Property<string>("WorkPhoneCode");

                    b.Property<DateTime?>("WorkPhoneLastUpdated");

                    b.HasKey("Id");

                    b.HasIndex("CountryCode");

                    b.HasIndex("LocalityId");

                    b.HasIndex("SublocalityId");

                    b.ToTable("MemberView","dbo");
                });

            modelBuilder.Entity("smartFunds.Data.Models.Contactbase.Region", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("RegionView","dbo");
                });

            modelBuilder.Entity("smartFunds.Data.Models.Contactbase.Sublocality", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Address1");

                    b.Property<string>("Address2");

                    b.Property<string>("Address3");

                    b.Property<string>("Address4");

                    b.Property<string>("Address5");

                    b.Property<string>("Address6");

                    b.Property<string>("Address7");

                    b.Property<string>("Address8");

                    b.Property<bool>("IsMainHall");

                    b.Property<int>("LocalityId");

                    b.Property<string>("Name");

                    b.Property<string>("ShortName");

                    b.HasKey("Id");

                    b.HasIndex("LocalityId");

                    b.ToTable("SublocalityView","dbo");
                });

            modelBuilder.Entity("smartFunds.Data.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CountryCode");

                    b.Property<DateTime>("DateLastUpdated");

                    b.Property<string>("DeletedAt")
                        .HasMaxLength(30);

                    b.Property<DateTime>("EventDate");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int>("MainLocalityId");

                    b.HasKey("Id");

                    b.HasIndex("CountryCode");

                    b.HasIndex("MainLocalityId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("smartFunds.Data.Models.EventHost", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CP");

                    b.Property<DateTime>("DateLastUpdated");

                    b.Property<string>("DeletedAt")
                        .HasMaxLength(30);

                    b.Property<int>("EventId");

                    b.Property<int>("HostId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int>("SCP");

                    b.HasKey("Id");

                    b.HasIndex("HostId");

                    b.HasIndex("EventId", "HostId");

                    b.ToTable("EventHosts");
                });

            modelBuilder.Entity("smartFunds.Data.Models.EventMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateLastUpdated");

                    b.Property<string>("DeletedAt")
                        .HasMaxLength(30);

                    b.Property<int>("EventId");

                    b.Property<int>("HouseholderId");

                    b.Property<bool>("IsAway");

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("IsGuest");

                    b.Property<bool>("IsHost");

                    b.Property<bool>("IsToBeAssigned");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int>("MemberId");

                    b.HasKey("Id");

                    b.HasIndex("EventId", "MemberId", "HouseholderId");

                    b.ToTable("EventMembers");
                });

            modelBuilder.Entity("smartFunds.Data.Models.EventSublocality", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateLastUpdated");

                    b.Property<string>("DeletedAt");

                    b.Property<int>("EventId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int>("SublocalityId");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("SublocalityId");

                    b.ToTable("EventSublocalities");
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

            modelBuilder.Entity("smartFunds.Data.Models.Host", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateLastUpdated");

                    b.Property<int>("DefaultCP");

                    b.Property<int>("DefaultSCP");

                    b.Property<int>("HouseholderId");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int>("LocalityId");

                    b.HasKey("Id");

                    b.HasIndex("HouseholderId");

                    b.ToTable("Hosts");
                });

            modelBuilder.Entity("smartFunds.Data.Models.Interchange", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CountryCode");

                    b.Property<DateTime>("DateLastUpdated");

                    b.Property<string>("DeletedAt")
                        .HasMaxLength(30);

                    b.Property<string>("EmailAddress");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int>("MainLocalityId");

                    b.HasKey("Id");

                    b.HasIndex("CountryCode");

                    b.HasIndex("MainLocalityId", "IsDeleted", "DeletedAt")
                        .IsUnique()
                        .HasFilter("[DeletedAt] IS NOT NULL");

                    b.ToTable("Interchanges");
                });

            modelBuilder.Entity("smartFunds.Data.Models.InterchangeLocality", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateLastUpdated");

                    b.Property<string>("DeletedAt")
                        .HasMaxLength(30);

                    b.Property<int>("InterchangeId");

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("LastUpdatedBy");

                    b.Property<int>("LocalityId");

                    b.HasKey("Id");

                    b.HasIndex("InterchangeId");

                    b.HasIndex("LocalityId");

                    b.ToTable("InterchangeLocalities");
                });

            modelBuilder.Entity("smartFunds.Data.Models.Setting", b =>
                {
                    b.Property<string>("Key")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Value");

                    b.HasKey("Key");

                    b.ToTable("Settings");
                });

            modelBuilder.Entity("smartFunds.Data.Models.UserQueuedJob", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("DateLastUpdated");

                    b.Property<string>("DeletedAt")
                        .HasMaxLength(30);

                    b.Property<bool>("IsDeleted");

                    b.Property<string>("JobId")
                        .IsRequired();

                    b.Property<string>("LastUpdatedBy")
                        .HasMaxLength(50);

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("UserQueuedJobs");
                });

            modelBuilder.Entity("smartFunds.Data.Models.Contactbase.Country", b =>
                {
                    b.HasOne("smartFunds.Data.Models.Contactbase.Region", "Region")
                        .WithMany()
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("smartFunds.Data.Models.Contactbase.Locality", b =>
                {
                    b.HasOne("smartFunds.Data.Models.Contactbase.Country")
                        .WithMany("Localities")
                        .HasForeignKey("CountryCode");
                });

            modelBuilder.Entity("smartFunds.Data.Models.Contactbase.Member", b =>
                {
                    b.HasOne("smartFunds.Data.Models.Contactbase.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryCode");

                    b.HasOne("smartFunds.Data.Models.Contactbase.Locality", "Locality")
                        .WithMany()
                        .HasForeignKey("LocalityId");

                    b.HasOne("smartFunds.Data.Models.Contactbase.Sublocality", "Sublocality")
                        .WithMany()
                        .HasForeignKey("SublocalityId");
                });

            modelBuilder.Entity("smartFunds.Data.Models.Contactbase.Sublocality", b =>
                {
                    b.HasOne("smartFunds.Data.Models.Contactbase.Locality", "Locality")
                        .WithMany("Sublocalities")
                        .HasForeignKey("LocalityId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("smartFunds.Data.Models.Event", b =>
                {
                    b.HasOne("smartFunds.Data.Models.Contactbase.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryCode");

                    b.HasOne("smartFunds.Data.Models.Contactbase.Locality", "Locality")
                        .WithMany()
                        .HasForeignKey("MainLocalityId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("smartFunds.Data.Models.EventHost", b =>
                {
                    b.HasOne("smartFunds.Data.Models.Event", "Event")
                        .WithMany("EventHosts")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("smartFunds.Data.Models.Host", "Host")
                        .WithMany("EventHosts")
                        .HasForeignKey("HostId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("smartFunds.Data.Models.EventMember", b =>
                {
                    b.HasOne("smartFunds.Data.Models.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("smartFunds.Data.Models.EventSublocality", b =>
                {
                    b.HasOne("smartFunds.Data.Models.Event", "Event")
                        .WithMany("EventSublocalities")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("smartFunds.Data.Models.Contactbase.Sublocality", "Sublocality")
                        .WithMany()
                        .HasForeignKey("SublocalityId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("smartFunds.Data.Models.Interchange", b =>
                {
                    b.HasOne("smartFunds.Data.Models.Contactbase.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryCode");

                    b.HasOne("smartFunds.Data.Models.Contactbase.Locality", "Locality")
                        .WithMany()
                        .HasForeignKey("MainLocalityId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("smartFunds.Data.Models.InterchangeLocality", b =>
                {
                    b.HasOne("smartFunds.Data.Models.Interchange", "Interchange")
                        .WithMany("InterchangeLocalities")
                        .HasForeignKey("InterchangeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("smartFunds.Data.Models.Contactbase.Locality", "Locality")
                        .WithMany()
                        .HasForeignKey("LocalityId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
