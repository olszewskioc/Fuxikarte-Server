using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fuxikarte.Backend.Migrations
{
    /// <inheritdoc />
    public partial class PaidInSale : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "paid",
                table: "sales",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "paid",
                table: "sales");
        }
    }
}
