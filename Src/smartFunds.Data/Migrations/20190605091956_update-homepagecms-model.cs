using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class updatehomepagecmsmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstBanner",
                table: "HomepageConfigurations");

            migrationBuilder.RenameColumn(
                name: "SecondBanner",
                table: "HomepageConfigurations",
                newName: "ImageName");

            migrationBuilder.RenameColumn(
                name: "PartnerBanner",
                table: "HomepageConfigurations",
                newName: "Category");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "HomepageConfigurations",
                newName: "SecondBanner");

            migrationBuilder.RenameColumn(
                name: "Category",
                table: "HomepageConfigurations",
                newName: "PartnerBanner");

            migrationBuilder.AddColumn<string>(
                name: "FirstBanner",
                table: "HomepageConfigurations",
                nullable: true);
        }
    }
}
