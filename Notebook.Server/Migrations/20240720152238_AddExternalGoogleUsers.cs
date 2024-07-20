using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notebook.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddExternalGoogleUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalGoogleUserId",
                table: "Notebooks",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExternalGoogleUsers",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExternalGoogleUsers", x => x.Email);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notebooks_ExternalGoogleUserId",
                table: "Notebooks",
                column: "ExternalGoogleUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notebooks_ExternalGoogleUsers_ExternalGoogleUserId",
                table: "Notebooks",
                column: "ExternalGoogleUserId",
                principalTable: "ExternalGoogleUsers",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notebooks_ExternalGoogleUsers_ExternalGoogleUserId",
                table: "Notebooks");

            migrationBuilder.DropTable(
                name: "ExternalGoogleUsers");

            migrationBuilder.DropIndex(
                name: "IX_Notebooks_ExternalGoogleUserId",
                table: "Notebooks");

            migrationBuilder.DropColumn(
                name: "ExternalGoogleUserId",
                table: "Notebooks");
        }
    }
}
