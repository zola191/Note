using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notebook.Server.Migrations
{
    /// <inheritdoc />
    public partial class changeModels20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RestoreAccount",
                table: "Accounts",
                newName: "RestoreAccountUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RestoreAccountUrl",
                table: "Accounts",
                newName: "RestoreAccount");
        }
    }
}
