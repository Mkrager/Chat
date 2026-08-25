using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Chat.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addCiphertextAndIvProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: new Guid("c99b6971-83a4-4b32-9a72-e7cf83f47c2f"));

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: new Guid("c99b6971-83a4-4b32-9a72-e7cf83f47c6f"));

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Messages",
                newName: "Iv");

            migrationBuilder.AddColumn<string>(
                name: "Ciphertext",
                table: "Messages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ciphertext",
                table: "Messages");

            migrationBuilder.RenameColumn(
                name: "Iv",
                table: "Messages",
                newName: "Content");

            migrationBuilder.InsertData(
                table: "Messages",
                columns: new[] { "Id", "Content", "SenderId", "SendDate", "ReceiverId" },
                values: new object[,]
                {
                    { new Guid("c99b6971-83a4-4b32-9a72-e7cf83f47c2f"), "Second message", new Guid("d385ac98-8c90-4946-9ab3-27f821fd7623"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("6e02e7bd-8f2e-4c25-9696-dad78a1307cb") },
                    { new Guid("c99b6971-83a4-4b32-9a72-e7cf83f47c6f"), "First message", new Guid("d385ac98-8c90-4946-9ab3-27f821fd7623"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("6e02e7bd-8f2e-4c25-9696-dad78a1307cb") }
                });
        }
    }
}
