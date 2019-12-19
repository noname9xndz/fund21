using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class UpdateFiedTransactionHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "TransactionHistory");

            migrationBuilder.RenameColumn(
                name: "TimeAction",
                table: "TransactionHistory",
                newName: "TransactionDate");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "TransactionHistory",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TransactionType",
                table: "TransactionHistory",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionType",
                table: "TransactionHistory");

            migrationBuilder.RenameColumn(
                name: "TransactionDate",
                table: "TransactionHistory",
                newName: "TimeAction");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "TransactionHistory",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "TransactionHistory",
                nullable: true);
        }
    }
}
