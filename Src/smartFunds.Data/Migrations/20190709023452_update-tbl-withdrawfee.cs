using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class updatetblwithdrawfee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //    migrationBuilder.AlterColumn<decimal>(
            //        name: "Percentage",
            //        table: "WithdrawFees",
            //        type: "decimal(3,4)",
            //        nullable: false,
            //        oldClrType: typeof(decimal));

            //    migrationBuilder.CreateTable(
            //        name: "TaskCompleted",
            //        columns: table => new
            //        {
            //            Id = table.Column<int>(nullable: false)
            //                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //            TaskType = table.Column<int>(nullable: false),
            //            ObjectID = table.Column<int>(nullable: false),
            //            ObjectName = table.Column<string>(nullable: true),
            //            TransactionAmount = table.Column<decimal>(nullable: false),
            //            LastUpdatedDate = table.Column<DateTime>(nullable: false),
            //            LastUpdatedBy = table.Column<string>(nullable: true)
            //        },
            //        constraints: table =>
            //        {
            //            table.PrimaryKey("PK_TaskCompleted", x => x.Id);
            //        });
        }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        //    migrationBuilder.DropTable(
        //        name: "TaskCompleted");

        //    migrationBuilder.AlterColumn<decimal>(
        //        name: "Percentage",
        //        table: "WithdrawFees",
        //        nullable: false,
        //        oldClrType: typeof(decimal),
        //        oldType: "decimal(3,4)");
    }
}
}
