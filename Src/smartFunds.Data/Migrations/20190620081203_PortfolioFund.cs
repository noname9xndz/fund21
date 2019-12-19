using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class PortfolioFund : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "FundPercent",
                table: "PortfolioFund",
                nullable: true,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FundPercentNew",
                table: "PortfolioFund",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FundPercentNew",
                table: "PortfolioFund");

            migrationBuilder.AlterColumn<decimal>(
                name: "FundPercent",
                table: "PortfolioFund",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);
        }
    }
}
