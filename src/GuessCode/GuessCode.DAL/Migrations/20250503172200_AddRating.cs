using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GuessCode.DAL.Migrations
{
    /// <inheritdoc />
    public partial class AddRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PointsReward",
                table: "Kata",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "KataCodeExecutionResult",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ExecutedBy = table.Column<long>(type: "bigint", nullable: false),
                    KataId = table.Column<long>(type: "bigint", nullable: false),
                    Output = table.Column<string>(type: "text", nullable: true),
                    Error = table.Column<string>(type: "text", nullable: true),
                    TotalTestCount = table.Column<int>(type: "integer", nullable: false),
                    PassedTestCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KataCodeExecutionResult", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KataCodeExecutionResult_Kata_KataId",
                        column: x => x.KataId,
                        principalTable: "Kata",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RatingChange",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RatingChangeValue = table.Column<long>(type: "bigint", nullable: false),
                    ChangeReason = table.Column<int>(type: "integer", nullable: false),
                    ChangedBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RatingChange", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RatingChange_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KataCodeExecutionResult_KataId",
                table: "KataCodeExecutionResult",
                column: "KataId");

            migrationBuilder.CreateIndex(
                name: "IX_RatingChange_UserId",
                table: "RatingChange",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KataCodeExecutionResult");

            migrationBuilder.DropTable(
                name: "RatingChange");

            migrationBuilder.DropColumn(
                name: "PointsReward",
                table: "Kata");
        }
    }
}
