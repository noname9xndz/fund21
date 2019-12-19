using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class addtablegenericintroducingnintroducingprofile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GenericIntroducingSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Banner = table.Column<string>(nullable: true),
                    BannerDescription = table.Column<string>(nullable: true),
                    FirstImage = table.Column<string>(nullable: true),
                    FirstImageDescription = table.Column<string>(nullable: true),
                    SecondImage = table.Column<string>(nullable: true),
                    SecondImageDescription = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenericIntroducingSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntroducingProfileSettings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ImageProfile = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    GenericIntroducingSettingId = table.Column<int>(nullable: true)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IntroducingProfileSettings");

            migrationBuilder.DropTable(
                name: "GenericIntroducingSettings");
        }
    }
}
