using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GuessCode.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixMentorLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mentor_User_UserId1",
                table: "Mentor");

            migrationBuilder.DropIndex(
                name: "IX_Mentor_UserId1",
                table: "Mentor");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Mentor");

            migrationBuilder.CreateIndex(
                name: "IX_Mentor_UserId",
                table: "Mentor",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Mentor_User_UserId",
                table: "Mentor",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mentor_User_UserId",
                table: "Mentor");

            migrationBuilder.DropIndex(
                name: "IX_Mentor_UserId",
                table: "Mentor");

            migrationBuilder.AddColumn<long>(
                name: "UserId1",
                table: "Mentor",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mentor_UserId1",
                table: "Mentor",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Mentor_User_UserId1",
                table: "Mentor",
                column: "UserId1",
                principalTable: "User",
                principalColumn: "Id");
        }
    }
}
