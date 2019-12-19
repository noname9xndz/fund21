using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class updatetblhomepageconfiguration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BannerDescription",
                table: "GenericIntroducingSettings");

            migrationBuilder.DropColumn(
                name: "FirstImage",
                table: "GenericIntroducingSettings");

            migrationBuilder.DropColumn(
                name: "FirstImageDescription",
                table: "GenericIntroducingSettings");

            migrationBuilder.DropColumn(
                name: "SecondImage",
                table: "GenericIntroducingSettings");

            migrationBuilder.RenameColumn(
                name: "SecondImageDescription",
                table: "GenericIntroducingSettings",
                newName: "Description");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "GenericIntroducingSettings",
                newName: "SecondImageDescription");

            migrationBuilder.AddColumn<string>(
                name: "BannerDescription",
                table: "GenericIntroducingSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstImage",
                table: "GenericIntroducingSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstImageDescription",
                table: "GenericIntroducingSettings",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondImage",
                table: "GenericIntroducingSettings",
                nullable: true);
        }
    }
}
