using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CrmInventory.Migrations
{
    /// <inheritdoc />
    public partial class AddUserExpenseRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "MetExpenses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MetExpenses_UserId",
                table: "MetExpenses",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MetExpenses_Users_UserId",
                table: "MetExpenses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MetExpenses_Users_UserId",
                table: "MetExpenses");

            migrationBuilder.DropIndex(
                name: "IX_MetExpenses_UserId",
                table: "MetExpenses");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "MetExpenses");
        }
    }
}
