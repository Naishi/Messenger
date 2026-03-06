using System;

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Messenger.Migrations
{
    /// <inheritdoc />
    public partial class LastMessagePropInChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Messages");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastMessageDate",
                table: "Chats",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastMessageDate",
                table: "Chats");

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Messages",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}