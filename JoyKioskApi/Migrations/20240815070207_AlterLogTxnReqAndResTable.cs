using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JoyKioskApi.Migrations
{
    /// <inheritdoc />
    public partial class AlterLogTxnReqAndResTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LogTxnResponses",
                table: "LogTxnResponses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LogTxnRequests",
                table: "LogTxnRequests");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LogTxnResponses",
                table: "LogTxnResponses",
                columns: new[] { "TxnDate", "TxnType", "PartnerTxnUid", "PartnerId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_LogTxnRequests",
                table: "LogTxnRequests",
                columns: new[] { "TxnDate", "TxnType", "PartnerTxnUid", "PartnerId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LogTxnResponses",
                table: "LogTxnResponses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LogTxnRequests",
                table: "LogTxnRequests");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LogTxnResponses",
                table: "LogTxnResponses",
                column: "TxnDate");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LogTxnRequests",
                table: "LogTxnRequests",
                column: "TxnDate");
        }
    }
}
