using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace daily_regex.Migrations
{
    /// <inheritdoc />
    public partial class Challenges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Challenges",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Solution = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatedById = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Challenges", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Challenges_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserAttempts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ChallengeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AttemptedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    RegexSubmitted = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAttempts", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ChallengeSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Date = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ChallengeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChallengeSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChallengeSchedules_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Challenges",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "Description", "Solution", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0a5e2320-0fcb-4ff6-8589-47b6882c134f"), new DateTime(2025, 8, 24, 9, 20, 10, 697, DateTimeKind.Utc).AddTicks(7986), null, "Valida una fecha en formato dd/mm/yyyy (ej: 23/08/2025).", "^(0[1-9]|[12][0-9]|3[01])/(0[1-9]|1[0-2])/\\d{4}$", "Fechas", new DateTime(2025, 8, 24, 9, 20, 10, 697, DateTimeKind.Utc).AddTicks(7986) },
                    { new Guid("33acd1ff-9920-4d74-9db5-b10d89df0ef1"), new DateTime(2025, 8, 24, 9, 20, 10, 697, DateTimeKind.Utc).AddTicks(7983), null, "Encuentra todos los hashtags en un texto (ej: #regex #coding).", "#\\w+", "Encuentra Hashtags", new DateTime(2025, 8, 24, 9, 20, 10, 697, DateTimeKind.Utc).AddTicks(7984) },
                    { new Guid("3d40cfc5-4bca-4314-8d4d-826e04da28d2"), new DateTime(2025, 8, 24, 9, 20, 10, 697, DateTimeKind.Utc).AddTicks(7990), null, "Valida una URL que empiece con http o https.", "^https?:\\/\\/[^\\s/$.?#].[^\\s]*$", "Validar URL", new DateTime(2025, 8, 24, 9, 20, 10, 697, DateTimeKind.Utc).AddTicks(7990) },
                    { new Guid("516995f7-5f70-4dd8-a4cc-ac4e638192e8"), new DateTime(2025, 8, 24, 9, 20, 10, 697, DateTimeKind.Utc).AddTicks(7356), null, "Encuentra un email válido (ejemplo: user@example.com).", "^[\\w\\.-]+@[\\w\\.-]+\\.\\w+$", "Validar Email", new DateTime(2025, 8, 24, 9, 20, 10, 697, DateTimeKind.Utc).AddTicks(7700) },
                    { new Guid("51c48f76-b168-45d1-a954-f5892030d5f8"), new DateTime(2025, 8, 24, 9, 20, 10, 697, DateTimeKind.Utc).AddTicks(7988), null, "Encuentra palabras que comiencen con vocal (ej: 'apple', 'orange').", "\\b[aeiouAEIOU]\\w*", "Palabras con vocal", new DateTime(2025, 8, 24, 9, 20, 10, 697, DateTimeKind.Utc).AddTicks(7988) },
                    { new Guid("dac555e9-1f4d-45b5-b28f-6e938bed32bf"), new DateTime(2025, 8, 24, 9, 20, 10, 697, DateTimeKind.Utc).AddTicks(7966), null, "Encuentra todos los números enteros en el texto (ej: 42, -7, 1234).", "-?\\d+", "Detectar enteros", new DateTime(2025, 8, 24, 9, 20, 10, 697, DateTimeKind.Utc).AddTicks(7966) },
                    { new Guid("e9957ea7-209e-493c-95d9-2c5f655681ec"), new DateTime(2025, 8, 24, 9, 20, 10, 697, DateTimeKind.Utc).AddTicks(7968), null, "Valida un código postal de 5 dígitos (ej: 28013).", "^\\d{5}$", "Código Postal", new DateTime(2025, 8, 24, 9, 20, 10, 697, DateTimeKind.Utc).AddTicks(7968) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Challenges_CreatedById",
                table: "Challenges",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ChallengeSchedules_ChallengeId",
                table: "ChallengeSchedules",
                column: "ChallengeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChallengeSchedules");

            migrationBuilder.DropTable(
                name: "UserAttempts");

            migrationBuilder.DropTable(
                name: "Challenges");
        }
    }
}
