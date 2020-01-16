using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class addImageKVRRQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageDesktop",
                table: "KvrrQuestion",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageMobile",
                table: "KvrrQuestion",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageDesktop",
                table: "KvrrQuestion");

            migrationBuilder.DropColumn(
                name: "ImageMobile",
                table: "KvrrQuestion");
        }
    }
}
