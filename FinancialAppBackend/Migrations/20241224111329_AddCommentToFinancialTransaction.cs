using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialAppBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddCommentToFinancialTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "FinancialTransactions",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "FinancialTransactions");
        }
    }
}
