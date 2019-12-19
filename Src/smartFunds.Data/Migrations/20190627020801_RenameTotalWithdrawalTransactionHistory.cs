using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class RenameTotalWithdrawalTransactionHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalWithdrawn",
                table: "TransactionHistory",
                newName: "TotalWithdrawal");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalWithdrawal",
                table: "TransactionHistory",
                newName: "TotalWithdrawn");
        }
    }
}
