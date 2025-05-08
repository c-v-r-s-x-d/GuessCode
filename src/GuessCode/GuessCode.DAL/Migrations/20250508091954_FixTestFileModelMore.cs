using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuessCode.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixTestFileModelMore : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "KataTestFile");

            migrationBuilder.AddColumn<Guid>(
                name: "FileId",
                table: "KataTestFile",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileId",
                table: "KataTestFile");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "KataTestFile",
                type: "text",
                nullable: true);
        }
    }
}
