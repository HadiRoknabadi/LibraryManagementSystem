using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class fix_borrowing_relation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Borrowings_BookCopyId",
                table: "Borrowings",
                column: "BookCopyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrowings_BookCopies_BookCopyId",
                table: "Borrowings",
                column: "BookCopyId",
                principalTable: "BookCopies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrowings_BookCopies_BookCopyId",
                table: "Borrowings");

            migrationBuilder.DropIndex(
                name: "IX_Borrowings_BookCopyId",
                table: "Borrowings");
        }
    }
}
