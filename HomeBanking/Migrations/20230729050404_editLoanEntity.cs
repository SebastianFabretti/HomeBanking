using Microsoft.EntityFrameworkCore.Migrations;

namespace HomeBanking.Migrations
{
    public partial class editLoanEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientLoans_Clients_ClientID",
                table: "ClientLoans");

            migrationBuilder.RenameColumn(
                name: "Payment",
                table: "Loans",
                newName: "Payments");

            migrationBuilder.RenameColumn(
                name: "ClientID",
                table: "ClientLoans",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_ClientLoans_ClientID",
                table: "ClientLoans",
                newName: "IX_ClientLoans_ClientId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientLoans_Clients_ClientId",
                table: "ClientLoans",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientLoans_Clients_ClientId",
                table: "ClientLoans");

            migrationBuilder.RenameColumn(
                name: "Payments",
                table: "Loans",
                newName: "Payment");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "ClientLoans",
                newName: "ClientID");

            migrationBuilder.RenameIndex(
                name: "IX_ClientLoans_ClientId",
                table: "ClientLoans",
                newName: "IX_ClientLoans_ClientID");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientLoans_Clients_ClientID",
                table: "ClientLoans",
                column: "ClientID",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
