using System.Collections.Generic;
using GuessCode.DAL.Models.Enums;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GuessCode.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddMentorLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MentorId",
                table: "User",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MemoryTaken",
                table: "KataCodeExecutionResult",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TimeElapsed",
                table: "KataCodeExecutionResult",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Kata",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Dictionary<ProgrammingLanguage, decimal>>(
                name: "MemoryLimits",
                table: "Kata",
                type: "jsonb",
                nullable: true);

            migrationBuilder.AddColumn<Dictionary<ProgrammingLanguage, decimal>>(
                name: "TimeLimits",
                table: "Kata",
                type: "jsonb",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "KataTestFile",
                columns: table => new
                {
                    KataId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    KataId1 = table.Column<long>(type: "bigint", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KataTestFile", x => x.KataId);
                    table.ForeignKey(
                        name: "FK_KataTestFile_Kata_KataId1",
                        column: x => x.KataId1,
                        principalTable: "Kata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Mentor",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    UserId1 = table.Column<long>(type: "bigint", nullable: true),
                    Experience = table.Column<long>(type: "bigint", nullable: false),
                    ProgrammingLanguages = table.Column<int[]>(type: "integer[]", nullable: true),
                    Availability = table.Column<int>(type: "integer", nullable: false),
                    About = table.Column<string>(type: "text", nullable: true),
                    Rating = table.Column<decimal>(type: "numeric", nullable: false, defaultValue: 0m),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mentor", x => x.Id);
                    table.CheckConstraint("CK_Review_Rating_Range", "\"Rating\" >= 0.0 AND \"Rating\" <= 5.0");
                    table.ForeignKey(
                        name: "FK_Mentor_User_UserId1",
                        column: x => x.UserId1,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MentorRequest",
                columns: table => new
                {
                    MentorId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MentorRequest", x => new { x.UserId, x.MentorId });
                    table.ForeignKey(
                        name: "FK_MentorRequest_Mentor_MentorId",
                        column: x => x.MentorId,
                        principalTable: "Mentor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MentorRequest_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_User_MentorId",
                table: "User",
                column: "MentorId");

            migrationBuilder.CreateIndex(
                name: "IX_KataTestFile_KataId1",
                table: "KataTestFile",
                column: "KataId1");

            migrationBuilder.CreateIndex(
                name: "IX_Mentor_UserId1",
                table: "Mentor",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_MentorRequest_MentorId",
                table: "MentorRequest",
                column: "MentorId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Mentor_MentorId",
                table: "User",
                column: "MentorId",
                principalTable: "Mentor",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Mentor_MentorId",
                table: "User");

            migrationBuilder.DropTable(
                name: "KataTestFile");

            migrationBuilder.DropTable(
                name: "MentorRequest");

            migrationBuilder.DropTable(
                name: "Mentor");

            migrationBuilder.DropIndex(
                name: "IX_User_MentorId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "MentorId",
                table: "User");

            migrationBuilder.DropColumn(
                name: "MemoryTaken",
                table: "KataCodeExecutionResult");

            migrationBuilder.DropColumn(
                name: "TimeElapsed",
                table: "KataCodeExecutionResult");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Kata");

            migrationBuilder.DropColumn(
                name: "MemoryLimits",
                table: "Kata");

            migrationBuilder.DropColumn(
                name: "TimeLimits",
                table: "Kata");
        }
    }
}
