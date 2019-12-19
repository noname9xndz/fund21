using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class SM_67_RenameEventMealSetupTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventMealSetups");

            migrationBuilder.CreateTable(
                name: "EventMembers",
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
                    table.PrimaryKey("PK_EventMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventMembers_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventMembers_EventId_MemberId_HouseholderId",
                table: "EventMembers",
                columns: new[] { "EventId", "MemberId", "HouseholderId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventMembers");

            migrationBuilder.CreateTable(
                name: "EventMealSetups",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateLastUpdated = table.Column<DateTime>(nullable: false),
                    DeletedAt = table.Column<string>(nullable: true),
                    EventId = table.Column<int>(nullable: false),
                    HouseholderId = table.Column<int>(nullable: false),
                    IsAway = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    IsGuest = table.Column<bool>(nullable: false),
                    IsHost = table.Column<bool>(nullable: false),
                    IsToBeAssigned = table.Column<bool>(nullable: false),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    MemberId = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_EventMealSetups_EventId_MemberId_HouseholderId",
                table: "EventMealSetups",
                columns: new[] { "EventId", "MemberId", "HouseholderId" });
        }
    }
}
