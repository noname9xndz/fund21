﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class RevertWithdrawFee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Percentage",
                table: "WithdrawFees",
                type: "decimal(12,4)",
                nullable: false,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Percentage",
                table: "WithdrawFees",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(12,4)");
        }
    }
}