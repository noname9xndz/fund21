using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class SM67_CreateNewTable_MealSetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventMealSetups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EventId = table.Column<int>(nullable: false),
                    MemberId = table.Column<int>(nullable: false),
                    HouseholderId = table.Column<int>(nullable: false),
                    IsHost = table.Column<bool>(nullable: false),
                    IsGuest = table.Column<bool>(nullable: false),
                    IsAway = table.Column<bool>(nullable: false),
                    IsToBeAssigned = table.Column<bool>(nullable: false),
                    DateLastUpdated = table.Column<DateTime>(nullable: false),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedAt = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventMealSetups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventMealSetups_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Hosts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    HouseholderId = table.Column<int>(nullable: false),
                    DefaultCP = table.Column<int>(nullable: false),
                    DefaultSCP = table.Column<int>(nullable: false),
                    LocalityId = table.Column<int>(nullable: false),
                    DateLastUpdated = table.Column<DateTime>(nullable: false),
                    LastUpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hosts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventHosts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EventId = table.Column<int>(nullable: false),
                    HostId = table.Column<int>(nullable: false),
                    CP = table.Column<int>(nullable: false),
                    SCP = table.Column<int>(nullable: false),
                    DateLastUpdated = table.Column<DateTime>(nullable: false),
                    LastUpdatedBy = table.Column<string>(nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_EventHosts_EventId_HostId",
                table: "EventHosts",
                columns: new[] { "EventId", "HostId" });

            migrationBuilder.CreateIndex(
                name: "IX_EventMealSetups_EventId_MemberId_HouseholderId",
                table: "EventMealSetups",
                columns: new[] { "EventId", "MemberId", "HouseholderId" });

            migrationBuilder.CreateIndex(
                name: "IX_Hosts_HouseholderId",
                table: "Hosts",
                column: "HouseholderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropForeignKey(
                name: "FK_InterchangeLocalities_LocalityView_LocalityId",
                table: "InterchangeLocalities");

            migrationBuilder.DropTable(
                name: "EventHosts");

            migrationBuilder.DropTable(
                name: "EventMealSetups");

            migrationBuilder.DropTable(
                name: "Hosts");
        }
    }
}
