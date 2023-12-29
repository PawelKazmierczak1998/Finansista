using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finansista.Data.Migrations
{
    public partial class Update1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "Balance",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Balance_userId",
                table: "Balance",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Balance_AspNetUsers_userId",
                table: "Balance",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Balance_AspNetUsers_userId",
                table: "Balance");

            migrationBuilder.DropIndex(
                name: "IX_Balance_userId",
                table: "Balance");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "Balance");
        }
    }
}
