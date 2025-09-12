using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Messenger.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteContactWhenLostCommunication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Users_ContactUserId",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Users_OwnerUserId",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Chats_ChatId",
                table: "Messages");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "UserAuth",
                type: "character varying(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Users_ContactUserId",
                table: "Contacts",
                column: "ContactUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Users_OwnerUserId",
                table: "Contacts",
                column: "OwnerUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Chats_ChatId",
                table: "Messages",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Users_ContactUserId",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Users_OwnerUserId",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Chats_ChatId",
                table: "Messages");

            migrationBuilder.AlterColumn<string>(
                name: "PasswordHash",
                table: "UserAuth",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(512)",
                oldMaxLength: 512);

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Users_ContactUserId",
                table: "Contacts",
                column: "ContactUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Users_OwnerUserId",
                table: "Contacts",
                column: "OwnerUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Chats_ChatId",
                table: "Messages",
                column: "ChatId",
                principalTable: "Chats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
