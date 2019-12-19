using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class createportfoliotablekvrrportfoliotable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_KvrrMark_KVRRId",
                table: "KvrrMark");

            migrationBuilder.CreateTable(
                name: "Portfolio",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    PercentOnKVRR = table.Column<double>(nullable: false),
                    DateLastUpdated = table.Column<DateTime>(nullable: false),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedAt = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portfolio", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KVRRPortfolio",
                columns: table => new
                {
                    KVRRId = table.Column<int>(nullable: false),
                    PortfolioId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KVRRPortfolio", x => new { x.KVRRId, x.PortfolioId });
                    table.ForeignKey(
                        name: "FK_KVRRPortfolio_Kvrr_KVRRId",
                        column: x => x.KVRRId,
                        principalTable: "Kvrr",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KVRRPortfolio_Portfolio_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KvrrMark_KVRRId",
                table: "KvrrMark",
                column: "KVRRId");

            migrationBuilder.CreateIndex(
                name: "IX_KVRRPortfolio_PortfolioId",
                table: "KVRRPortfolio",
                column: "PortfolioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KVRRPortfolio");

            migrationBuilder.DropTable(
                name: "Portfolio");

            migrationBuilder.DropIndex(
                name: "IX_KvrrMark_KVRRId",
                table: "KvrrMark");

            migrationBuilder.CreateIndex(
                name: "IX_KvrrMark_KVRRId",
                table: "KvrrMark",
                column: "KVRRId",
                unique: true,
                filter: "[KVRRId] IS NOT NULL");
        }
    }
}
