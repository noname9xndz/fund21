using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class UpdateTableKVRRName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KVRRAnswer_KVRRQuestions_KVRRQuestionId",
                table: "KVRRAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KVRRQuestions",
                table: "KVRRQuestions");

            migrationBuilder.RenameTable(
                name: "KVRRQuestions",
                newName: "KvrrQuestion");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KvrrQuestion",
                table: "KvrrQuestion",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_KVRRAnswer_KvrrQuestion_KVRRQuestionId",
                table: "KVRRAnswer",
                column: "KVRRQuestionId",
                principalTable: "KvrrQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KVRRAnswer_KvrrQuestion_KVRRQuestionId",
                table: "KVRRAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KvrrQuestion",
                table: "KvrrQuestion");

            migrationBuilder.RenameTable(
                name: "KvrrQuestion",
                newName: "KVRRQuestions");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KVRRQuestions",
                table: "KVRRQuestions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_KVRRAnswer_KVRRQuestions_KVRRQuestionId",
                table: "KVRRAnswer",
                column: "KVRRQuestionId",
                principalTable: "KVRRQuestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
