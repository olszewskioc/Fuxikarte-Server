using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fuxikarte.Backend.Migrations
{
    /// <inheritdoc />
    public partial class CreateOnDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_categories_category_id",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "FK_sales_customers_customer_id",
                table: "sales");

            migrationBuilder.DropForeignKey(
                name: "FK_sales_locals_local_id",
                table: "sales");

            migrationBuilder.AlterColumn<int>(
                name: "local_id",
                table: "sales",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "customer_id",
                table: "sales",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_products_categories_category_id",
                table: "products",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "category_id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_sales_customers_customer_id",
                table: "sales",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "customer_id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_sales_locals_local_id",
                table: "sales",
                column: "local_id",
                principalTable: "locals",
                principalColumn: "local_id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_products_categories_category_id",
                table: "products");

            migrationBuilder.DropForeignKey(
                name: "FK_sales_customers_customer_id",
                table: "sales");

            migrationBuilder.DropForeignKey(
                name: "FK_sales_locals_local_id",
                table: "sales");

            migrationBuilder.AlterColumn<int>(
                name: "local_id",
                table: "sales",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "customer_id",
                table: "sales",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_products_categories_category_id",
                table: "products",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "category_id");

            migrationBuilder.AddForeignKey(
                name: "FK_sales_customers_customer_id",
                table: "sales",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "customer_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sales_locals_local_id",
                table: "sales",
                column: "local_id",
                principalTable: "locals",
                principalColumn: "local_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
