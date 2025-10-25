using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class RefactorCategoryFinalCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_sub_category_Categories_category_id",
                table: "sub_category");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ImageUrlCategory",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "category");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "category",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "category",
                newName: "id");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "category",
                type: "varchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_category",
                table: "category",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_category_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "category",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sub_category_category_category_id",
                table: "sub_category",
                column: "category_id",
                principalTable: "category",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_category_CategoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_sub_category_category_category_id",
                table: "sub_category");

            migrationBuilder.DropPrimaryKey(
                name: "PK_category",
                table: "category");

            migrationBuilder.RenameTable(
                name: "category",
                newName: "Categories");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Categories",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Categories",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(32)",
                oldMaxLength: 32)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrlCategory",
                table: "Categories",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sub_category_Categories_category_id",
                table: "sub_category",
                column: "category_id",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
