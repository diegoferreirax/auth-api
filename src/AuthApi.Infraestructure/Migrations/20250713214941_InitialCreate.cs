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
                    { "2e355313-89d3-4274-a25b-e1ffe372dfbb", "U", "USER", new DateTime(2025, 7, 13, 18, 49, 40, 298, DateTimeKind.Local).AddTicks(2409) },
                    { "4535ace9-c485-4c16-b086-3f7b3f48a08b", "M", "MANAGER", new DateTime(2025, 7, 13, 18, 49, 40, 298, DateTimeKind.Local).AddTicks(2411) },
                    { "6b4441ca-a23a-4ec4-b3bd-a1ca1507d9de", "A", "ADMIN", new DateTime(2025, 7, 13, 18, 49, 40, 298, DateTimeKind.Local).AddTicks(2366) }
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
                name: "ROLE");

            migrationBuilder.DropTable(
                name: "USER_ROLE");

            migrationBuilder.DropTable(
                name: "USER");
        }
    }
}
