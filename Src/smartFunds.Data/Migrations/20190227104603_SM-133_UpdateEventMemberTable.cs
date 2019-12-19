using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class SM133_UpdateEventMemberTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGuest",
                table: "EventMembers");

            migrationBuilder.DropColumn(
                name: "IsHost",
                table: "EventMembers");

            migrationBuilder.AddColumn<int>(
                name: "HouseholderId",
                table: "EventHosts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HouseholderId",
                table: "EventHosts");

            migrationBuilder.AddColumn<bool>(
                name: "IsGuest",
                table: "EventMembers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsHost",
                table: "EventMembers",
                nullable: false,
                defaultValue: false);
        }
    }
}
