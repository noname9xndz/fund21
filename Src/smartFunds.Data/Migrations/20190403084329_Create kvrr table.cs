using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace smartFunds.Data.Migrations
{
    public partial class Createkvrrtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KVRRAnswer_KvrrQuestion_KVRRQuestionId",
                table: "KVRRAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KVRRAnswer",
                table: "KVRRAnswer");

            migrationBuilder.RenameTable(
                name: "KVRRAnswer",
                newName: "KvrrAnswer");

            migrationBuilder.RenameIndex(
                name: "IX_KVRRAnswer_KVRRQuestionId",
                table: "KvrrAnswer",
                newName: "IX_KvrrAnswer_KVRRQuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KvrrAnswer",
                table: "KvrrAnswer",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Kvrr",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Detail = table.Column<string>(nullable: true),
                    KVRRImagePath = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DeletedAt = table.Column<string>(maxLength: 30, nullable: true),
                    DateLastUpdated = table.Column<DateTime>(nullable: false),
                    LastUpdatedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kvrr", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KvrrMark",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MarkFrom = table.Column<int>(nullable: false),
                    MarkTo = table.Column<int>(nullable: false),
                    KVRRId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KvrrMark", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KvrrMark_Kvrr_KVRRId",
                        column: x => x.KVRRId,
                        principalTable: "Kvrr",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KvrrMark_KVRRId",
                table: "KvrrMark",
                column: "KVRRId",
                unique: true,
                filter: "[KVRRId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_KvrrAnswer_KvrrQuestion_KVRRQuestionId",
                table: "KvrrAnswer",
                column: "KVRRQuestionId",
                principalTable: "KvrrQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KvrrAnswer_KvrrQuestion_KVRRQuestionId",
                table: "KvrrAnswer");

            migrationBuilder.DropTable(
                name: "KvrrMark");

            migrationBuilder.DropTable(
                name: "Kvrr");

            migrationBuilder.DropPrimaryKey(
                name: "PK_KvrrAnswer",
                table: "KvrrAnswer");

            migrationBuilder.RenameTable(
                name: "KvrrAnswer",
                newName: "KVRRAnswer");

            migrationBuilder.RenameIndex(
                name: "IX_KvrrAnswer_KVRRQuestionId",
                table: "KVRRAnswer",
                newName: "IX_KVRRAnswer_KVRRQuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_KVRRAnswer",
                table: "KVRRAnswer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_KVRRAnswer_KvrrQuestion_KVRRQuestionId",
                table: "KVRRAnswer",
                column: "KVRRQuestionId",
                principalTable: "KvrrQuestion",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
