using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class user : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "KVRRId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_KVRRId",
                table: "AspNetUsers",
                column: "KVRRId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Kvrr_KVRRId",
                table: "AspNetUsers",
                column: "KVRRId",
                principalTable: "Kvrr",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Kvrr_KVRRId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_KVRRId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "KVRRId",
                table: "AspNetUsers");
        }
    }
}
