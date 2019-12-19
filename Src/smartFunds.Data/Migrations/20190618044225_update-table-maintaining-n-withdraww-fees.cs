using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class updatetablemaintainingnwithdrawwfees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeInvestment",
                table: "WithdrawFees");

            migrationBuilder.RenameColumn(
                name: "CurrentAmountRanges",
                table: "MaintainingFees",
                newName: "AmountTo");

            migrationBuilder.AddColumn<int>(
                name: "TimeInvestmentBegin",
                table: "WithdrawFees",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TimeInvestmentEnd",
                table: "WithdrawFees",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "AmountFrom",
                table: "MaintainingFees",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeInvestmentBegin",
                table: "WithdrawFees");

            migrationBuilder.DropColumn(
                name: "TimeInvestmentEnd",
                table: "WithdrawFees");

            migrationBuilder.DropColumn(
                name: "AmountFrom",
                table: "MaintainingFees");

            migrationBuilder.RenameColumn(
                name: "AmountTo",
                table: "MaintainingFees",
                newName: "CurrentAmountRanges");

            migrationBuilder.AddColumn<decimal>(
                name: "TimeInvestment",
                table: "WithdrawFees",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
