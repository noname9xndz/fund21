using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class addtablecustomlevelv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomerLevel",
                columns: table => new
                {
                    IDCustomerLevel = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NameCustomerLevel = table.Column<string>(nullable: true),
                    MinMoney = table.Column<decimal>(nullable: false),
                    MaxMoney = table.Column<decimal>(nullable: false),
                    DateLastUpdated = table.Column<DateTime>(nullable: false),
                    LastUpdatedBy = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedAt = table.Column<string>(nullable: true),
                    CertificateValue = table.Column<decimal>(nullable: false),
                    NAV = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerLevel", x => x.IDCustomerLevel);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerLevel");
        }
    }
}
