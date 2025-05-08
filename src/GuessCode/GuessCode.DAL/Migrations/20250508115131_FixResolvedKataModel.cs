using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuessCode.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixResolvedKataModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ExecutedAt",
                table: "KataCodeExecutionResult",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SourceCode",
                table: "KataCodeExecutionResult",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExecutedAt",
                table: "KataCodeExecutionResult");

            migrationBuilder.DropColumn(
                name: "SourceCode",
                table: "KataCodeExecutionResult");
        }
    }
}
