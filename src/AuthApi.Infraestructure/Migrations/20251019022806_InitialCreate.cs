using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

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
                    CODE = table.Column<string>(type: "VARCHAR(10)", nullable: false)
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
                values: new object[] { "da650913-9937-4732-a1e6-86da6ca42d8d", "UM", "User Manager", new DateTime(2025, 10, 19, 2, 28, 6, 233, DateTimeKind.Utc).AddTicks(3848) });

            migrationBuilder.InsertData(
                table: "USER",
                columns: new[] { "ID", "EMAIL", "HASH", "NAME", "UPDATED_DATE" },
                values: new object[] { "e8ebb9f7-8c8e-49e8-952d-aaf56e88a055", "usermanager@gmail.com", "$2a$11$uNGxjs/ErX9ro.1SqQKVOeoANXftn18GFpshWP7XjP.fItQKWY7bm", "User Manager", new DateTime(2025, 10, 19, 2, 28, 6, 233, DateTimeKind.Utc).AddTicks(3971) });

            migrationBuilder.InsertData(
                table: "USER_ROLE",
                columns: new[] { "ID", "ID_ROLE", "ID_USER" },
                values: new object[] { new Guid("dc22d6be-ae9d-473c-9eda-0567e5952291"), "da650913-9937-4732-a1e6-86da6ca42d8d", "e8ebb9f7-8c8e-49e8-952d-aaf56e88a055" });

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
