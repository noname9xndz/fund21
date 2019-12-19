using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class addFundTransactionHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "FundTransactionHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    FundId = table.Column<int>(nullable: true),
                    Amount = table.Column<decimal>(nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FundTransactionHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FundTransactionHistory_Fund_FundId",
                        column: x => x.FundId,
                        principalTable: "Fund",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FundTransactionHistory_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FundTransactionHistory_FundId",
                table: "FundTransactionHistory",
                column: "FundId");

            migrationBuilder.CreateIndex(
                name: "IX_FundTransactionHistory_UserId",
                table: "FundTransactionHistory",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FundTransactionHistory");

            migrationBuilder.DropColumn(
                name: "TotalInvestAmount",
                table: "Fund");

            migrationBuilder.DropColumn(
                name: "TotalWithdrawnAmount",
                table: "Fund");

            migrationBuilder.CreateTable(
                name: "BalanceFunds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FundId = table.Column<int>(nullable: true),
                    Period = table.Column<int>(nullable: false),
                    TotalInvestAmount = table.Column<decimal>(nullable: false),
                    TotalWithdrawnAmount = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BalanceFunds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BalanceFunds_Fund_FundId",
                        column: x => x.FundId,
                        principalTable: "Fund",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BalanceFundDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Amount = table.Column<decimal>(nullable: false),
                    BalanceFundId = table.Column<int>(nullable: true),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BalanceFundDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BalanceFundDetails_BalanceFunds_BalanceFundId",
                        column: x => x.BalanceFundId,
                        principalTable: "BalanceFunds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BalanceFundDetails_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BalanceFundDetails_BalanceFundId",
                table: "BalanceFundDetails",
                column: "BalanceFundId");

            migrationBuilder.CreateIndex(
                name: "IX_BalanceFundDetails_UserId",
                table: "BalanceFundDetails",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BalanceFunds_FundId",
                table: "BalanceFunds",
                column: "FundId");
        }
    }
}
