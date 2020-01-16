using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class contentnews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CateNews",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ParentNewsID = table.Column<int>(nullable: false),
                    CateNewsName = table.Column<string>(type: "NVARCHAR(500)", nullable: true),
                    CateNewsOrder = table.Column<int>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    Contents = table.Column<string>(type: "NVARCHAR(1000)", nullable: true),
                    Images = table.Column<string>(type: "VARCHAR(500)", nullable: true),
                    DateLastUpdated = table.Column<DateTime>(nullable: false),
                    LastUpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CateNews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContentNews",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CateNewsID = table.Column<int>(nullable: false),
                    Title = table.Column<string>(type: "NVARCHAR(500)", nullable: true),
                    ShortDescribe = table.Column<string>(type: "NVARCHAR(500)", nullable: true),
                    Contents = table.Column<string>(type: "NTEXT", nullable: true),
                    ImageThumb = table.Column<string>(type: "NVARCHAR(500)", nullable: true),
                    ImageLarge = table.Column<string>(type: "NVARCHAR(500)", nullable: true),
                    FileName = table.Column<string>(type: "NVARCHAR(250)", nullable: true),
                    Author = table.Column<string>(type: "NVARCHAR(200)", nullable: true),
                    PostDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<bool>(nullable: false),
                    Ishome = table.Column<bool>(nullable: false),
                    DateLastUpdated = table.Column<DateTime>(nullable: false),
                    LastUpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentNews", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CateNews");

            migrationBuilder.DropTable(
                name: "ContentNews");
        }
    }
}
