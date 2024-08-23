using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notebook.Server.Migrations
{
    /// <inheritdoc />
    public partial class seedAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Email", "FirstName", "LastName", "PasswordHash", "Salt" },
                values: new object[] { "admin@notebook.com", null, null, "C47F53CE089299984780A3859E1309AA8383E09EFC92F777BE2F6B69086300FAF4F97C012701E22830C105A341811F4897E8400AB0A361B7120F6E1CDF224C45", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "admin@notebook.com");
        }
    }
}
