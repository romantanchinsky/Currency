using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace User.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial_UserSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "user_account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_account", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "user_account",
                columns: new[] { "Id", "Name", "PasswordHash" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "user1", "$2a$11$Hx78qfxbF7NE/rwfxVbWc.ikwBMD1vAW/PjZnOFs4IXWm3W9kICdq" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "user2", "$2a$11$Hx78qfxbF7NE/rwfxVbWc.ikwBMD1vAW/PjZnOFs4IXWm3W9kICdq" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_user_account_Name",
                table: "user_account",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "user_account");
        }
    }
}
