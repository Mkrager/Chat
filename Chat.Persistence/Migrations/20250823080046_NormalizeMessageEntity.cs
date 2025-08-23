using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class NormalizeMessageEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReceiverUserId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "SenderUserName",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Messages",
                newName: "ReceiverId");

            migrationBuilder.AddColumn<string>(
                name: "SenderId",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: new Guid("c99b6971-83a4-4b32-9a72-e7cf83f47c6f"),
                columns: new[] { "SenderId", "ReceiverId" },
                values: new object[] { "d385ac98-8c90-4946-9ab3-27f821fd7623", "" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "ReceiverId",
                table: "Messages",
                newName: "UserId");

            migrationBuilder.AddColumn<string>(
                name: "ReceiverUserId",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SenderUserName",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: new Guid("c99b6971-83a4-4b32-9a72-e7cf83f47c6f"),
                columns: new[] { "ReceiverUserId", "SenderUserName", "UserId" },
                values: new object[] { "", "", "845aa16b-fb09-45da-8a8a-fdb1deb48ee8" });
        }
    }
}
