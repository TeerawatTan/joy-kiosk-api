using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoyKioskApi.Migrations
{
    /// <inheritdoc />
    public partial class CreateLogTxnResponseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogTxnResponses",
                columns: table => new
                {
                    TxnDate = table.Column<DateTime>(type: "timestamp", nullable: false),
                    TxnType = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PartnerTxnUid = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PartnerId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    StatusCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogTxnResponses", x => x.TxnDate);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogTxnResponses");
        }
    }
}
