using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class HangFireJobCreateTableIgnore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //  migrationBuilder.EnsureSchema(
            //    name: "HangFire");

            //  migrationBuilder.CreateTable(
            //    name: "Job",
            //    schema: "HangFire",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        StateName = table.Column<string>(nullable: true),
            //        CreatedAt = table.Column<DateTime>(nullable: false),
            //        ExpireAt = table.Column<DateTime>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Job", x => x.Id);
            //    });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.DropTable(
            //    name: "Job",
            //    schema: "HangFire");
        }
    }
}
