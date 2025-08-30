using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace daily_regex.Migrations
{
    /// <inheritdoc />
    public partial class testCases : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Challenges",
                keyColumn: "Id",
                keyValue: new Guid("0a5e2320-0fcb-4ff6-8589-47b6882c134f"));

            migrationBuilder.DeleteData(
                table: "Challenges",
                keyColumn: "Id",
                keyValue: new Guid("33acd1ff-9920-4d74-9db5-b10d89df0ef1"));

            migrationBuilder.DeleteData(
                table: "Challenges",
                keyColumn: "Id",
                keyValue: new Guid("3d40cfc5-4bca-4314-8d4d-826e04da28d2"));

            migrationBuilder.DeleteData(
                table: "Challenges",
                keyColumn: "Id",
                keyValue: new Guid("516995f7-5f70-4dd8-a4cc-ac4e638192e8"));

            migrationBuilder.DeleteData(
                table: "Challenges",
                keyColumn: "Id",
                keyValue: new Guid("51c48f76-b168-45d1-a954-f5892030d5f8"));

            migrationBuilder.DeleteData(
                table: "Challenges",
                keyColumn: "Id",
                keyValue: new Guid("dac555e9-1f4d-45b5-b28f-6e938bed32bf"));

            migrationBuilder.DeleteData(
                table: "Challenges",
                keyColumn: "Id",
                keyValue: new Guid("e9957ea7-209e-493c-95d9-2c5f655681ec"));

            migrationBuilder.CreateTable(
                name: "TestCases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Text = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ChallengeId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestCases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestCases_Challenges_ChallengeId",
                        column: x => x.ChallengeId,
                        principalTable: "Challenges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Challenges",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "Description", "Solution", "Title", "UpdatedAt" },
                values: new object[] { new Guid("224cde2d-e7e0-4b4a-aad0-d23007ab461b"), new DateTime(2025, 8, 25, 22, 47, 8, 166, DateTimeKind.Utc).AddTicks(4137), null, "Encuentra un email válido (ejemplo: user@example.com).", "^[\\w\\.-]+@[\\w\\.-]+\\.\\w+$", "Validar Email", new DateTime(2025, 8, 25, 22, 47, 8, 166, DateTimeKind.Utc).AddTicks(4378) });

            migrationBuilder.InsertData(
                table: "TestCases",
                columns: new[] { "Id", "ChallengeId", "Text" },
                values: new object[,]
                {
                    { new Guid("092a01c5-a379-411a-b582-af8a2cf2f3fd"), new Guid("224cde2d-e7e0-4b4a-aad0-d23007ab461b"), "user@example.com" },
                    { new Guid("d968266f-f5ff-4e33-a116-874460c80c87"), new Guid("224cde2d-e7e0-4b4a-aad0-d23007ab461b"), "not-an-email" },
                    { new Guid("fbecbb3b-6238-41ec-83ea-8bcb944dc152"), new Guid("224cde2d-e7e0-4b4a-aad0-d23007ab461b"), "john.doe@domain.co.uk" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_TestCases_ChallengeId",
                table: "TestCases",
                column: "ChallengeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TestCases");

            migrationBuilder.DeleteData(
                table: "Challenges",
                keyColumn: "Id",
                keyValue: new Guid("224cde2d-e7e0-4b4a-aad0-d23007ab461b"));

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
        }
    }
}
