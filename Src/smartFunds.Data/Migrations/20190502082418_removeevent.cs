using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class removeevent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventSublocalities");

            migrationBuilder.DropTable(
                name: "MealAllocations");

            migrationBuilder.DropTable(
                name: "EventGuests");

            migrationBuilder.DropTable(
                name: "EventHosts");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Hosts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CountryCode = table.Column<string>(nullable: true),
                    DateLastUpdated = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<string>(maxLength: 30, nullable: true),
                    EventDate = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    MainLocalityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_CountryView_CountryCode",
                        column: x => x.CountryCode,
                        principalSchema: "dbo",
                        principalTable: "CountryView",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Events_LocalityView_MainLocalityId",
                        column: x => x.MainLocalityId,
                        principalSchema: "dbo",
                        principalTable: "LocalityView",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Hosts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateLastUpdated = table.Column<DateTime>(nullable: false),
                    DefaultCP = table.Column<int>(nullable: false),
                    DefaultSCP = table.Column<int>(nullable: false),
                    HouseholderId = table.Column<int>(nullable: false),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    LocalityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hosts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventGuests",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateLastUpdated = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<string>(maxLength: 30, nullable: true),
                    EventId = table.Column<int>(nullable: false),
                    HouseholderId = table.Column<int>(nullable: false),
                    IsAway = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsToBeAssigned = table.Column<bool>(nullable: false),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    MemberId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventGuests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventGuests_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventSublocalities",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateLastUpdated = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<string>(nullable: true),
                    EventId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    SublocalityId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventSublocalities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventSublocalities_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventSublocalities_SublocalityView_SublocalityId",
                        column: x => x.SublocalityId,
                        principalSchema: "dbo",
                        principalTable: "SublocalityView",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventHosts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CP = table.Column<int>(nullable: false),
                    DateLastUpdated = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<string>(maxLength: 30, nullable: true),
                    EventId = table.Column<int>(nullable: false),
                    HostId = table.Column<int>(nullable: false),
                    HouseholderId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    SCP = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventHosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventHosts_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EventHosts_Hosts_HostId",
                        column: x => x.HostId,
                        principalTable: "Hosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MealAllocations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateLastUpdated = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<string>(nullable: true),
                    EventGuestId = table.Column<int>(nullable: false),
                    EventHostId = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    LastUpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealAllocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MealAllocations_EventGuests_EventGuestId",
                        column: x => x.EventGuestId,
                        principalTable: "EventGuests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MealAllocations_EventHosts_EventHostId",
                        column: x => x.EventHostId,
                        principalTable: "EventHosts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventGuests_EventId_MemberId_HouseholderId",
                table: "EventGuests",
                columns: new[] { "EventId", "MemberId", "HouseholderId" });

            migrationBuilder.CreateIndex(
                name: "IX_EventHosts_HostId",
                table: "EventHosts",
                column: "HostId");

            migrationBuilder.CreateIndex(
                name: "IX_EventHosts_EventId_HostId",
                table: "EventHosts",
                columns: new[] { "EventId", "HostId" });

            migrationBuilder.CreateIndex(
                name: "IX_Events_CountryCode",
                table: "Events",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_Events_MainLocalityId",
                table: "Events",
                column: "MainLocalityId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSublocalities_EventId",
                table: "EventSublocalities",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_EventSublocalities_SublocalityId",
                table: "EventSublocalities",
                column: "SublocalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Hosts_HouseholderId",
                table: "Hosts",
                column: "HouseholderId");

            migrationBuilder.CreateIndex(
                name: "IX_MealAllocations_EventGuestId",
                table: "MealAllocations",
                column: "EventGuestId");

            migrationBuilder.CreateIndex(
                name: "IX_MealAllocations_EventHostId",
                table: "MealAllocations",
                column: "EventHostId");
        }
    }
}
