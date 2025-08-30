using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace daily_regex.Migrations
{
    /// <inheritdoc />
    public partial class testCasesMatchProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TestCases",
                keyColumn: "Id",
                keyValue: new Guid("092a01c5-a379-411a-b582-af8a2cf2f3fd"));

            migrationBuilder.DeleteData(
                table: "TestCases",
                keyColumn: "Id",
                keyValue: new Guid("d968266f-f5ff-4e33-a116-874460c80c87"));

            migrationBuilder.DeleteData(
                table: "TestCases",
                keyColumn: "Id",
                keyValue: new Guid("fbecbb3b-6238-41ec-83ea-8bcb944dc152"));

            migrationBuilder.DeleteData(
                table: "Challenges",
                keyColumn: "Id",
                keyValue: new Guid("224cde2d-e7e0-4b4a-aad0-d23007ab461b"));

            migrationBuilder.AddColumn<bool>(
                name: "IsMatch",
                table: "TestCases",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "Challenges",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "Description", "Solution", "Title", "UpdatedAt" },
                values: new object[] { new Guid("8bc516d4-a738-43a2-a06a-e239806b5276"), new DateTime(2025, 8, 25, 23, 17, 6, 720, DateTimeKind.Utc).AddTicks(5146), null, "Encuentra un email válido (ejemplo: user@example.com).", "^[\\w\\.-]+@[\\w\\.-]+\\.\\w+$", "Validar Email", new DateTime(2025, 8, 25, 23, 17, 6, 720, DateTimeKind.Utc).AddTicks(5391) });

            migrationBuilder.InsertData(
                table: "TestCases",
                columns: new[] { "Id", "ChallengeId", "IsMatch", "Text" },
                values: new object[,]
                {
                    { new Guid("1e15dae0-bba1-4731-bb76-a766f876588e"), new Guid("8bc516d4-a738-43a2-a06a-e239806b5276"), true, "john.doe@domain.co.uk" },
                    { new Guid("53f4e881-75c2-4a53-8250-6d75ae5a1a0a"), new Guid("8bc516d4-a738-43a2-a06a-e239806b5276"), false, "not-an-email" },
                    { new Guid("7e8bd5bc-2fdd-4795-a3b7-fd976fdaf4a7"), new Guid("8bc516d4-a738-43a2-a06a-e239806b5276"), true, "user@example.com" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TestCases",
                keyColumn: "Id",
                keyValue: new Guid("1e15dae0-bba1-4731-bb76-a766f876588e"));

            migrationBuilder.DeleteData(
                table: "TestCases",
                keyColumn: "Id",
                keyValue: new Guid("53f4e881-75c2-4a53-8250-6d75ae5a1a0a"));

            migrationBuilder.DeleteData(
                table: "TestCases",
                keyColumn: "Id",
                keyValue: new Guid("7e8bd5bc-2fdd-4795-a3b7-fd976fdaf4a7"));

            migrationBuilder.DeleteData(
                table: "Challenges",
                keyColumn: "Id",
                keyValue: new Guid("8bc516d4-a738-43a2-a06a-e239806b5276"));

            migrationBuilder.DropColumn(
                name: "IsMatch",
                table: "TestCases");

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
        }
    }
}
