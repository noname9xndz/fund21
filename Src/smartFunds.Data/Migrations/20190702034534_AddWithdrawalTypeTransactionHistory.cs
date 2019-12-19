using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class AddWithdrawalTypeTransactionHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RemittanceStatus",
                table: "TransactionHistory",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WithdrawalType",
                table: "TransactionHistory",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemittanceStatus",
                table: "TransactionHistory");

            migrationBuilder.DropColumn(
                name: "WithdrawalType",
                table: "TransactionHistory");
        }
    }
}
