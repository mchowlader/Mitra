using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mitra.Api.Migrations
{
    /// <inheritdoc />
    public partial class updateproperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreteBy",
                table: "RefreshTokens",
                newName: "CreateBy");

            migrationBuilder.RenameColumn(
                name: "LoginTime",
                table: "LoginHistory",
                newName: "LogInTime");

            migrationBuilder.RenameColumn(
                name: "LoginOut",
                table: "LoginHistory",
                newName: "LogOutTime");

            migrationBuilder.RenameColumn(
                name: "CreteBy",
                table: "LoginHistory",
                newName: "CreateBy");

            migrationBuilder.RenameColumn(
                name: "CreteBy",
                table: "Action",
                newName: "CreateBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateBy",
                table: "RefreshTokens",
                newName: "CreteBy");

            migrationBuilder.RenameColumn(
                name: "LogInTime",
                table: "LoginHistory",
                newName: "LoginTime");

            migrationBuilder.RenameColumn(
                name: "LogOutTime",
                table: "LoginHistory",
                newName: "LoginOut");

            migrationBuilder.RenameColumn(
                name: "CreateBy",
                table: "LoginHistory",
                newName: "CreteBy");

            migrationBuilder.RenameColumn(
                name: "CreateBy",
                table: "Action",
                newName: "CreteBy");
        }
    }
}
