using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class updateFundCertificate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalWithdrawnAmount",
                table: "FundTransactionHistory",
                newName: "TotalWithdrawnNoOfCertificates");

            migrationBuilder.RenameColumn(
                name: "TotalInvestAmount",
                table: "FundTransactionHistory",
                newName: "TotalInvestNoOfCertificates");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "FundTransactionHistory",
                newName: "NoOfCertificates");

            migrationBuilder.RenameColumn(
                name: "CertificateValue",
                table: "Fund",
                newName: "NAVNew");

            migrationBuilder.AddColumn<int>(
                name: "EditStatus",
                table: "UserFunds",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EditStatus",
                table: "Fund",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EditStatus",
                table: "UserFunds");

            migrationBuilder.DropColumn(
                name: "EditStatus",
                table: "Fund");

            migrationBuilder.RenameColumn(
                name: "TotalWithdrawnNoOfCertificates",
                table: "FundTransactionHistory",
                newName: "TotalWithdrawnAmount");

            migrationBuilder.RenameColumn(
                name: "TotalInvestNoOfCertificates",
                table: "FundTransactionHistory",
                newName: "TotalInvestAmount");

            migrationBuilder.RenameColumn(
                name: "NoOfCertificates",
                table: "FundTransactionHistory",
                newName: "Amount");

            migrationBuilder.RenameColumn(
                name: "NAVNew",
                table: "Fund",
                newName: "CertificateValue");
        }
    }
}
