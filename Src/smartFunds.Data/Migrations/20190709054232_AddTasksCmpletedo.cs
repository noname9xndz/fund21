using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class AddTasksCmpletedo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskCompleted",
                table: "TaskCompleted");

            migrationBuilder.RenameTable(
                name: "TaskCompleted",
                newName: "TasksCompleted");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TasksCompleted",
                table: "TasksCompleted",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TasksCompleted",
                table: "TasksCompleted");

            migrationBuilder.RenameTable(
                name: "TasksCompleted",
                newName: "TaskCompleted");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskCompleted",
                table: "TaskCompleted",
                column: "Id");
        }
    }
}
