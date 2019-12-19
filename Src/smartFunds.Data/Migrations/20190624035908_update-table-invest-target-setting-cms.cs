using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class updatetableinvesttargetsettingcms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FiveYear",
                table: "InvestmentTargetSettings");

            migrationBuilder.DropColumn(
                name: "SixMonth",
                table: "InvestmentTargetSettings");

            migrationBuilder.DropColumn(
                name: "ThreeMonth",
                table: "InvestmentTargetSettings");

            migrationBuilder.DropColumn(
                name: "TwelveMonth",
                table: "InvestmentTargetSettings");

            migrationBuilder.DropColumn(
                name: "TwoYear",
                table: "InvestmentTargetSettings");

            migrationBuilder.AlterColumn<int>(
                name: "PortfolioId",
                table: "InvestmentTargetSettings",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "InvestmentTargetSettings",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "Value",
                table: "InvestmentTargetSettings",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "InvestmentTargetSettings");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "InvestmentTargetSettings");

            migrationBuilder.AlterColumn<int>(
                name: "PortfolioId",
                table: "InvestmentTargetSettings",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<float>(
                name: "FiveYear",
                table: "InvestmentTargetSettings",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "SixMonth",
                table: "InvestmentTargetSettings",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "ThreeMonth",
                table: "InvestmentTargetSettings",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TwelveMonth",
                table: "InvestmentTargetSettings",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TwoYear",
                table: "InvestmentTargetSettings",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
