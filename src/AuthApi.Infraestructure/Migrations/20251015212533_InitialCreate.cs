using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AuthApi.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ROLE",
                columns: table => new
                {
                    ID = table.Column<string>(type: "VARCHAR(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NAME = table.Column<string>(type: "VARCHAR(150)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CODE = table.Column<string>(type: "CHAR(1)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UPDATED_DATE = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ROLE", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "USER",
                columns: table => new
                {
                    ID = table.Column<string>(type: "VARCHAR(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NAME = table.Column<string>(type: "VARCHAR(150)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EMAIL = table.Column<string>(type: "VARCHAR(150)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    HASH = table.Column<string>(type: "VARCHAR(100)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UPDATED_DATE = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER", x => x.ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "USER_ROLE",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ID_USER = table.Column<string>(type: "VARCHAR(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ID_ROLE = table.Column<string>(type: "VARCHAR(36)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER_ROLE", x => x.ID);
                    table.UniqueConstraint("AK_USER_ROLE_ID_USER_ID_ROLE", x => new { x.ID_USER, x.ID_ROLE });
                    table.ForeignKey(
                        name: "FK_USER_ROLE_ROLE_ID_ROLE",
                        column: x => x.ID_ROLE,
                        principalTable: "ROLE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_USER_ROLE_USER_ID_USER",
                        column: x => x.ID_USER,
                        principalTable: "USER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "ROLE",
                columns: new[] { "ID", "CODE", "NAME", "UPDATED_DATE" },
                values: new object[,]
                {
                    { "11111111-1111-1111-1111-111111111111", "A", "ADMIN", new DateTime(2025, 10, 15, 21, 25, 33, 582, DateTimeKind.Utc).AddTicks(4945) },
                    { "22222222-2222-2222-2222-222222222222", "U", "USER", new DateTime(2025, 10, 15, 21, 25, 33, 582, DateTimeKind.Utc).AddTicks(4947) },
                    { "33333333-3333-3333-3333-333333333333", "M", "MANAGER", new DateTime(2025, 10, 15, 21, 25, 33, 582, DateTimeKind.Utc).AddTicks(4948) }
                });

            migrationBuilder.InsertData(
                table: "USER",
                columns: new[] { "ID", "EMAIL", "HASH", "NAME", "UPDATED_DATE" },
                values: new object[,]
                {
                    { "44444444-4444-4444-4444-444444444444", "admin@authapi.com", "BCrypt.Net.BCrypt.HashPassword(admin123)", "Administrador", new DateTime(2025, 10, 15, 21, 25, 33, 582, DateTimeKind.Utc).AddTicks(5067) },
                    { "55555555-5555-5555-5555-555555555555", "user@authapi.com", "BCrypt.Net.BCrypt.HashPassword(admin123)", "Usuário Padrão", new DateTime(2025, 10, 15, 21, 25, 33, 582, DateTimeKind.Utc).AddTicks(5069) },
                    { "66666666-6666-6666-6666-666666666666", "manager@authapi.com", "BCrypt.Net.BCrypt.HashPassword(admin123)", "Gerente", new DateTime(2025, 10, 15, 21, 25, 33, 582, DateTimeKind.Utc).AddTicks(5071) }
                });

            migrationBuilder.InsertData(
                table: "USER_ROLE",
                columns: new[] { "ID", "ID_ROLE", "ID_USER" },
                values: new object[,]
                {
                    { new Guid("77777777-7777-7777-7777-777777777777"), "11111111-1111-1111-1111-111111111111", "44444444-4444-4444-4444-444444444444" },
                    { new Guid("88888888-8888-8888-8888-888888888888"), "22222222-2222-2222-2222-222222222222", "55555555-5555-5555-5555-555555555555" },
                    { new Guid("99999999-9999-9999-9999-999999999999"), "33333333-3333-3333-3333-333333333333", "66666666-6666-6666-6666-666666666666" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "22222222-2222-2222-2222-222222222222", "66666666-6666-6666-6666-666666666666" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_USER_EMAIL",
                table: "USER",
                column: "EMAIL",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USER_ROLE_ID_ROLE",
                table: "USER_ROLE",
                column: "ID_ROLE");

            migrationBuilder.CreateIndex(
                name: "IX_USER_ROLE_ID_USER",
                table: "USER_ROLE",
                column: "ID_USER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "USER_ROLE");

            migrationBuilder.DropTable(
                name: "ROLE");

            migrationBuilder.DropTable(
                name: "USER");
        }
    }
}
