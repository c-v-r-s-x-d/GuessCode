using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GuessCode.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixTestFileModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KataTestFile_Kata_KataId1",
                table: "KataTestFile");

            migrationBuilder.DropIndex(
                name: "IX_KataTestFile_KataId1",
                table: "KataTestFile");

            migrationBuilder.DropColumn(
                name: "KataId1",
                table: "KataTestFile");

            migrationBuilder.AlterColumn<long>(
                name: "KataId",
                table: "KataTestFile",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_KataTestFile_Kata_KataId",
                table: "KataTestFile",
                column: "KataId",
                principalTable: "Kata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_KataTestFile_Kata_KataId",
                table: "KataTestFile");

            migrationBuilder.AlterColumn<long>(
                name: "KataId",
                table: "KataTestFile",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<long>(
                name: "KataId1",
                table: "KataTestFile",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_KataTestFile_KataId1",
                table: "KataTestFile",
                column: "KataId1");

            migrationBuilder.AddForeignKey(
                name: "FK_KataTestFile_Kata_KataId1",
                table: "KataTestFile",
                column: "KataId1",
                principalTable: "Kata",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
