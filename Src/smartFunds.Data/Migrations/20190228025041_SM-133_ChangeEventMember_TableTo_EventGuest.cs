using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class SM133_ChangeEventMember_TableTo_EventGuest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventMembers_Events_EventId",
                table: "EventMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventMembers",
                table: "EventMembers");

            migrationBuilder.RenameTable(
                name: "EventMembers",
                newName: "EventGuests");

            migrationBuilder.RenameIndex(
                name: "IX_EventMembers_EventId_MemberId_HouseholderId",
                table: "EventGuests",
                newName: "IX_EventGuests_EventId_MemberId_HouseholderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventGuests",
                table: "EventGuests",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventGuests_Events_EventId",
                table: "EventGuests",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventGuests_Events_EventId",
                table: "EventGuests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventGuests",
                table: "EventGuests");

            migrationBuilder.RenameTable(
                name: "EventGuests",
                newName: "EventMembers");

            migrationBuilder.RenameIndex(
                name: "IX_EventGuests_EventId_MemberId_HouseholderId",
                table: "EventMembers",
                newName: "IX_EventMembers_EventId_MemberId_HouseholderId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventMembers",
                table: "EventMembers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EventMembers_Events_EventId",
                table: "EventMembers",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
