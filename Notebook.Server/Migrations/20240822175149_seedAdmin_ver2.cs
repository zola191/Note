using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notebook.Server.Migrations
{
    /// <inheritdoc />
    public partial class seedAdmin_ver2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "admin@notebook.com",
                columns: new[] { "PasswordHash", "Salt" },
                values: new object[] { "A6C1558D933A3F6FF2A0F030591332D8AE4863F3473D4AB4BBF0933FB678157489DEDE17AE3564B5B900AD76E2E56BA980C7B3587E2FD5BE896821A564D9E04D", "F630C3DF9BAA8AF920B5FB5344C07014E21B7B545475DACB1CD9847736402DA7AE5A2A73F290B5D3E54B44801219276285E2371D29475BCD8AA95577A893E4BC" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "admin@notebook.com",
                columns: new[] { "PasswordHash", "Salt" },
                values: new object[] { "C47F53CE089299984780A3859E1309AA8383E09EFC92F777BE2F6B69086300FAF4F97C012701E22830C105A341811F4897E8400AB0A361B7120F6E1CDF224C45", null });
        }
    }
}
