using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoyKioskApi.Migrations
{
    /// <inheritdoc />
    public partial class AlterTableUserTokenAddCrmToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CrmToken",
                table: "UserTokens",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CrmToken",
                table: "UserTokens");
        }
    }
}
