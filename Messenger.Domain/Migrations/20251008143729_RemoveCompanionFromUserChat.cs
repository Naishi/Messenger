using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Messenger.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCompanionFromUserChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserChats_Users_CompanionId",
                table: "UserChats");

            migrationBuilder.DropIndex(
                name: "IX_UserChats_CompanionId",
                table: "UserChats");

            migrationBuilder.DropColumn(
                name: "CompanionId",
                table: "UserChats");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanionId",
                table: "UserChats",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserChats_CompanionId",
                table: "UserChats",
                column: "CompanionId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserChats_Users_CompanionId",
                table: "UserChats",
                column: "CompanionId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}