using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class addtblinvestmenttagetsetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvestmentTargetSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PortfolioId = table.Column<int>(nullable: true),
                    ThreeMonth = table.Column<float>(nullable: false),
                    SixMonth = table.Column<float>(nullable: false),
                    TwelveMonth = table.Column<float>(nullable: false),
                    TwoYear = table.Column<float>(nullable: false),
                    FiveYear = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentTargetSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentTargetSettings_Portfolio_PortfolioId",
                        column: x => x.PortfolioId,
                        principalTable: "Portfolio",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentTargetSettings_PortfolioId",
                table: "InvestmentTargetSettings",
                column: "PortfolioId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InvestmentTargetSettings");
        }
    }
}
