using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class CreateTableDefineKVRR : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KVRRQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: true),
                    No = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KVRRQuestions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KVRRAnswer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: true),
                    Mark = table.Column<int>(nullable: false),
                    KVRRQuestionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KVRRAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KVRRAnswer_KVRRQuestions_KVRRQuestionId",
                        column: x => x.KVRRQuestionId,
                        principalTable: "KVRRQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KVRRAnswer_KVRRQuestionId",
                table: "KVRRAnswer",
                column: "KVRRQuestionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KVRRAnswer");

            migrationBuilder.DropTable(
                name: "KVRRQuestions");
        }
    }
}
