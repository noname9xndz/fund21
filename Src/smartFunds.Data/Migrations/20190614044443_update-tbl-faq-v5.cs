using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class updatetblfaqv5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IntroducingProfileSettings");

            migrationBuilder.DropColumn(
                name: "FAQCategoryId",
                table: "FAQs");

            migrationBuilder.AddColumn<int>(
                name: "Category",
                table: "FAQs",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "FAQs");

            migrationBuilder.AddColumn<int>(
                name: "FAQCategoryId",
                table: "FAQs",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "IntroducingProfileSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GenericIntroducingSettingId = table.Column<int>(nullable: true),
                    ImageProfile = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntroducingProfileSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntroducingProfileSettings_GenericIntroducingSettings_GenericIntroducingSettingId",
                        column: x => x.GenericIntroducingSettingId,
                        principalTable: "GenericIntroducingSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_IntroducingProfileSettings_GenericIntroducingSettingId",
                table: "IntroducingProfileSettings",
                column: "GenericIntroducingSettingId");
        }
    }
}
