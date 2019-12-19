using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class updateBalanceFund : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalInvestAmount",
                table: "Fund");

            migrationBuilder.DropColumn(
                name: "TotalWithdrawnAmount",
                table: "Fund");

            migrationBuilder.AddColumn<string>(
                name: "LastUpdatedBy",
                table: "FundTransactionHistory",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedDate",
                table: "FundTransactionHistory",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "FundTransactionHistory",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalInvestAmount",
                table: "FundTransactionHistory",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalWithdrawnAmount",
                table: "FundTransactionHistory",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FrequencyUpdateSetting");

            migrationBuilder.DropTable(
                name: "IntroducingProfileSettings");

            migrationBuilder.DropTable(
                name: "InvestmentTargetSettings");

            migrationBuilder.DropTable(
                name: "GenericIntroducingSettings");

            migrationBuilder.DropColumn(
                name: "LastUpdatedBy",
                table: "FundTransactionHistory");

            migrationBuilder.DropColumn(
                name: "LastUpdatedDate",
                table: "FundTransactionHistory");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "FundTransactionHistory");

            migrationBuilder.DropColumn(
                name: "TotalInvestAmount",
                table: "FundTransactionHistory");

            migrationBuilder.DropColumn(
                name: "TotalWithdrawnAmount",
                table: "FundTransactionHistory");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalInvestAmount",
                table: "Fund",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalWithdrawnAmount",
                table: "Fund",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
