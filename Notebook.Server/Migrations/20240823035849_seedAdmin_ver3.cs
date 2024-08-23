using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Notebook.Server.Migrations
{
    /// <inheritdoc />
    public partial class seedAdmin_ver3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleUser");

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    RolesId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => new { x.RolesId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_UserRole_Roles_RolesId",
                        column: x => x.RolesId,
                        principalTable: "Roles",
                        principalColumn: "RoleName",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "UserRole",
                columns: new[] { "RolesId", "UsersId" },
                values: new object[] { 0, "admin@notebook.com" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "admin@notebook.com",
                columns: new[] { "PasswordHash", "Salt" },
                values: new object[] { "6143B1724B300E6E0E598F1D9014F0DD7B5839F4DE31384EA805E4AF09670F48497108BB060D8862B0AC9DB51BEC04CCB0BE676173717903731EB4743DEC880E", "1F815381DDF3CC57CDC54E29E8F22F4FA257C7A15010856B93C4F94AE7E26489DA0794EADF43187FBCA33E2EF5F12DD7A6264C45905BCDAA6E457F49612D5845" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UsersId",
                table: "UserRole",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRole");

            migrationBuilder.CreateTable(
                name: "RoleUser",
                columns: table => new
                {
                    RolesRoleName = table.Column<int>(type: "int", nullable: false),
                    UserEmail = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUser", x => new { x.RolesRoleName, x.UserEmail });
                    table.ForeignKey(
                        name: "FK_RoleUser_Roles_RolesRoleName",
                        column: x => x.RolesRoleName,
                        principalTable: "Roles",
                        principalColumn: "RoleName",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUser_Users_UserEmail",
                        column: x => x.UserEmail,
                        principalTable: "Users",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Email",
                keyValue: "admin@notebook.com",
                columns: new[] { "PasswordHash", "Salt" },
                values: new object[] { "A6C1558D933A3F6FF2A0F030591332D8AE4863F3473D4AB4BBF0933FB678157489DEDE17AE3564B5B900AD76E2E56BA980C7B3587E2FD5BE896821A564D9E04D", "F630C3DF9BAA8AF920B5FB5344C07014E21B7B545475DACB1CD9847736402DA7AE5A2A73F290B5D3E54B44801219276285E2371D29475BCD8AA95577A893E4BC" });

            migrationBuilder.CreateIndex(
                name: "IX_RoleUser_UserEmail",
                table: "RoleUser",
                column: "UserEmail");
        }
    }
}
