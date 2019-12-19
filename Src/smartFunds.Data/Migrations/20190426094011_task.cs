using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class task : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PercentOnKVRR",
                table: "Portfolio");          

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TransactionType = table.Column<int>(nullable: false),
                    FundId = table.Column<int>(nullable: false),
                    TransactionAmount = table.Column<decimal>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    LastUpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Fund_FundId",
                        column: x => x.FundId,
                        principalTable: "Fund",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
         

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_FundId",
                table: "Tasks",
                column: "FundId");
           
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {         

            migrationBuilder.DropTable(
                name: "Tasks");      

            migrationBuilder.AddColumn<double>(
                name: "PercentOnKVRR",
                table: "Portfolio",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
