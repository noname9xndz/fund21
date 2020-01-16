using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class UpdateUserAdjustmentFactor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentInvestmentAmount",
                table: "AspNetUsers",
                type: "decimal(18,5)",
                nullable: false);

            migrationBuilder.AlterColumn<decimal>(
                name: "AdjustmentFactor",
                table: "AspNetUsers",
                type: "decimal(18,12)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(15,7)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "CurrentInvestmentAmount",
                table: "AspNetUsers",
                type: "decimal(21,5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,5)");

            migrationBuilder.AlterColumn<decimal>(
                name: "AdjustmentFactor",
                table: "AspNetUsers",
                type: "decimal(20,12)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,12)");
        }
    }
}
