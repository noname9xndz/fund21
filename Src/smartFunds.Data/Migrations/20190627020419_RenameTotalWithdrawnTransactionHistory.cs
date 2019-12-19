using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class RenameTotalWithdrawnTransactionHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalInvestment",
                table: "TransactionHistory",
                newName: "TotalWithdrawn");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalWithdrawn",
                table: "TransactionHistory",
                newName: "TotalInvestment");
        }
    }
}
