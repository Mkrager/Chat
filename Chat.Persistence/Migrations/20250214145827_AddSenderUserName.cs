using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Chat.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddSenderUserName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                column: "SenderUserName",
                value: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SenderUserName",
                table: "Messages");
        }
    }
}
