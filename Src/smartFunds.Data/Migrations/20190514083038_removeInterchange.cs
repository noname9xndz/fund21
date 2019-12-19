using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class removeInterchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InterchangeLocalities");

            //migrationBuilder.DropTable(
            //    name: "MemberView",
            //    schema: "dbo");

            migrationBuilder.DropTable(
                name: "Interchanges");

            //migrationBuilder.DropTable(
            //    name: "SublocalityView",
            //    schema: "dbo");

            //migrationBuilder.DropTable(
            //    name: "LocalityView",
            //    schema: "dbo");

            //migrationBuilder.DropTable(
            //    name: "CountryView",
            //    schema: "dbo");

            //migrationBuilder.DropTable(
            //    name: "RegionView",
            //    schema: "dbo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "RegionView",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionView", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CountryView",
                schema: "dbo",
                columns: table => new
                {
                    Code = table.Column<string>(nullable: false),
                    Address1Label = table.Column<string>(nullable: true),
                    Address1Required = table.Column<bool>(nullable: false),
                    Address1Visible = table.Column<bool>(nullable: false),
                    Address2Label = table.Column<string>(nullable: true),
                    Address2Required = table.Column<bool>(nullable: false),
                    Address2Visible = table.Column<bool>(nullable: false),
                    Address3Label = table.Column<string>(nullable: true),
                    Address3Required = table.Column<bool>(nullable: false),
                    Address3Visible = table.Column<bool>(nullable: false),
                    Address4Label = table.Column<string>(nullable: true),
                    Address4Required = table.Column<bool>(nullable: false),
                    Address4Visible = table.Column<bool>(nullable: false),
                    Address5Label = table.Column<string>(nullable: true),
                    Address5Required = table.Column<bool>(nullable: false),
                    Address5Visible = table.Column<bool>(nullable: false),
                    Address6Label = table.Column<string>(nullable: true),
                    Address6Required = table.Column<bool>(nullable: false),
                    Address6Visible = table.Column<bool>(nullable: false),
                    Address7Label = table.Column<string>(nullable: true),
                    Address7Required = table.Column<bool>(nullable: false),
                    Address7Visible = table.Column<bool>(nullable: false),
                    Address8Label = table.Column<string>(nullable: true),
                    Address8Required = table.Column<bool>(nullable: false),
                    Address8Visible = table.Column<bool>(nullable: false),
                    AddressFormat = table.Column<string>(nullable: true),
                    DialingPrefix = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    RegionId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryView", x => x.Code);
                    table.ForeignKey(
                        name: "FK_CountryView_RegionView_RegionId",
                        column: x => x.RegionId,
                        principalSchema: "dbo",
                        principalTable: "RegionView",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LocalityView",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CountryCode = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalityView", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocalityView_CountryView_CountryCode",
                        column: x => x.CountryCode,
                        principalSchema: "dbo",
                        principalTable: "CountryView",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Interchanges",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CountryCode = table.Column<string>(nullable: true),
                    DateLastUpdated = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<string>(maxLength: 30, nullable: true),
                    EmailAddress = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    MainLocalityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interchanges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Interchanges_CountryView_CountryCode",
                        column: x => x.CountryCode,
                        principalSchema: "dbo",
                        principalTable: "CountryView",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Interchanges_LocalityView_MainLocalityId",
                        column: x => x.MainLocalityId,
                        principalSchema: "dbo",
                        principalTable: "LocalityView",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SublocalityView",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Address1 = table.Column<string>(nullable: true),
                    Address2 = table.Column<string>(nullable: true),
                    Address3 = table.Column<string>(nullable: true),
                    Address4 = table.Column<string>(nullable: true),
                    Address5 = table.Column<string>(nullable: true),
                    Address6 = table.Column<string>(nullable: true),
                    Address7 = table.Column<string>(nullable: true),
                    Address8 = table.Column<string>(nullable: true),
                    IsMainHall = table.Column<bool>(nullable: false),
                    LocalityId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    ShortName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SublocalityView", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SublocalityView_LocalityView_LocalityId",
                        column: x => x.LocalityId,
                        principalSchema: "dbo",
                        principalTable: "LocalityView",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InterchangeLocalities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateLastUpdated = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<string>(maxLength: 30, nullable: true),
                    InterchangeId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    LocalityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterchangeLocalities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterchangeLocalities_Interchanges_InterchangeId",
                        column: x => x.InterchangeId,
                        principalTable: "Interchanges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InterchangeLocalities_LocalityView_LocalityId",
                        column: x => x.LocalityId,
                        principalSchema: "dbo",
                        principalTable: "LocalityView",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MemberView",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Address1 = table.Column<string>(nullable: true),
                    Address2 = table.Column<string>(nullable: true),
                    Address3 = table.Column<string>(nullable: true),
                    Address4 = table.Column<string>(nullable: true),
                    Address5 = table.Column<string>(nullable: true),
                    Address6 = table.Column<string>(nullable: true),
                    Address7 = table.Column<string>(nullable: true),
                    Address8 = table.Column<string>(nullable: true),
                    AddressFormat = table.Column<string>(nullable: true),
                    Age = table.Column<int>(nullable: true),
                    CountryCode = table.Column<string>(nullable: true),
                    CountryName = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: true),
                    DeceasedDate = table.Column<DateTime>(nullable: true),
                    DeceasedSpouse = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    FatherDeceased = table.Column<bool>(nullable: true),
                    FatherId = table.Column<int>(nullable: true),
                    FathersLocality = table.Column<string>(nullable: true),
                    FathersName = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    FirstName_SC = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    FullNameReverse = table.Column<string>(nullable: true),
                    Generation = table.Column<string>(nullable: true),
                    HomePhone = table.Column<string>(nullable: true),
                    HomePhoneCode = table.Column<string>(nullable: true),
                    HomePhoneLastUpdated = table.Column<DateTime>(nullable: true),
                    HouseholdCoupleName = table.Column<string>(nullable: true),
                    HouseholdLastName = table.Column<string>(nullable: true),
                    HouseholdName = table.Column<string>(nullable: true),
                    HouseholdNameDisplay = table.Column<string>(nullable: true),
                    Household_FirstName = table.Column<string>(nullable: true),
                    Household_LastName = table.Column<string>(nullable: true),
                    HouseholderId = table.Column<int>(nullable: true),
                    HouseholderName = table.Column<string>(nullable: true),
                    IsDeceased = table.Column<bool>(nullable: false),
                    IsHidden = table.Column<bool>(nullable: false),
                    IsHouseholder = table.Column<bool>(nullable: false),
                    LastName = table.Column<string>(nullable: true),
                    LastName_SC = table.Column<string>(nullable: true),
                    LocalityId = table.Column<int>(nullable: true),
                    LocalityName = table.Column<string>(nullable: true),
                    MaidenName = table.Column<string>(nullable: true),
                    MobilePhone = table.Column<string>(nullable: true),
                    MobilePhoneCode = table.Column<string>(nullable: true),
                    MobilePhoneLastUpdated = table.Column<DateTime>(nullable: true),
                    MotherDeceased = table.Column<bool>(nullable: true),
                    MotherId = table.Column<int>(nullable: true),
                    MothersLocality = table.Column<string>(nullable: true),
                    MothersName = table.Column<string>(nullable: true),
                    PhotoTagCdnPath = table.Column<string>(nullable: true),
                    RegionId = table.Column<int>(nullable: true),
                    RegionName = table.Column<string>(nullable: true),
                    SpouseId = table.Column<int>(nullable: true),
                    SublocalityId = table.Column<int>(nullable: true),
                    SublocalityIsMainHall = table.Column<bool>(nullable: true),
                    SublocalityName = table.Column<string>(nullable: true),
                    SublocalityShortName = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    WorkPhone = table.Column<string>(nullable: true),
                    WorkPhoneCode = table.Column<string>(nullable: true),
                    WorkPhoneLastUpdated = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberView", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MemberView_CountryView_CountryCode",
                        column: x => x.CountryCode,
                        principalSchema: "dbo",
                        principalTable: "CountryView",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberView_LocalityView_LocalityId",
                        column: x => x.LocalityId,
                        principalSchema: "dbo",
                        principalTable: "LocalityView",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberView_SublocalityView_SublocalityId",
                        column: x => x.SublocalityId,
                        principalSchema: "dbo",
                        principalTable: "SublocalityView",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InterchangeLocalities_InterchangeId",
                table: "InterchangeLocalities",
                column: "InterchangeId");

            migrationBuilder.CreateIndex(
                name: "IX_InterchangeLocalities_LocalityId",
                table: "InterchangeLocalities",
                column: "LocalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Interchanges_CountryCode",
                table: "Interchanges",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Interchanges_MainLocalityId_IsDeleted_DeletedAt",
                table: "Interchanges",
                columns: new[] { "MainLocalityId", "IsDeleted", "DeletedAt" },
                unique: true,
                filter: "[DeletedAt] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CountryView_RegionId",
                schema: "dbo",
                table: "CountryView",
                column: "RegionId");

            migrationBuilder.CreateIndex(
                name: "IX_LocalityView_CountryCode",
                schema: "dbo",
                table: "LocalityView",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_MemberView_CountryCode",
                schema: "dbo",
                table: "MemberView",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_MemberView_LocalityId",
                schema: "dbo",
                table: "MemberView",
                column: "LocalityId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberView_SublocalityId",
                schema: "dbo",
                table: "MemberView",
                column: "SublocalityId");

            migrationBuilder.CreateIndex(
                name: "IX_SublocalityView_LocalityId",
                schema: "dbo",
                table: "SublocalityView",
                column: "LocalityId");
        }
    }
}
