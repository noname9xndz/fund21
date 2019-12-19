using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class updateFAQ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FAQs_FAQs_FAQId",
                table: "FAQs");

            migrationBuilder.DropIndex(
                name: "IX_FAQs_FAQId",
                table: "FAQs");

            migrationBuilder.DropColumn(
                name: "FAQId",
                table: "FAQs");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "FAQs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "FAQs");

            migrationBuilder.AddColumn<int>(
                name: "FAQId",
                table: "FAQs",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_FAQs_FAQId",
                table: "FAQs",
                column: "FAQId");

            migrationBuilder.AddForeignKey(
                name: "FK_FAQs_FAQs_FAQId",
                table: "FAQs",
                column: "FAQId",
                principalTable: "FAQs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
