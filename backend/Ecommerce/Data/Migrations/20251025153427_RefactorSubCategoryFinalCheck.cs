using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class RefactorSubCategoryFinalCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_SubCategories_SubCategoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCategories_Categories_ParentCategoryId",
                table: "SubCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubCategories",
                table: "SubCategories");

            migrationBuilder.RenameTable(
                name: "SubCategories",
                newName: "sub_category");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "sub_category",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "sub_category",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "ParentCategoryId",
                table: "sub_category",
                newName: "category_id");

            migrationBuilder.RenameIndex(
                name: "IX_SubCategories_ParentCategoryId",
                table: "sub_category",
                newName: "IX_sub_category_category_id");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "sub_category",
                type: "varchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_sub_category",
                table: "sub_category",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_sub_category_SubCategoryId",
                table: "Products",
                column: "SubCategoryId",
                principalTable: "sub_category",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_sub_category_Categories_category_id",
                table: "sub_category",
                column: "category_id",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_sub_category_SubCategoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_sub_category_Categories_category_id",
                table: "sub_category");

            migrationBuilder.DropPrimaryKey(
                name: "PK_sub_category",
                table: "sub_category");

            migrationBuilder.RenameTable(
                name: "sub_category",
                newName: "SubCategories");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "SubCategories",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "SubCategories",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "category_id",
                table: "SubCategories",
                newName: "ParentCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_sub_category_category_id",
                table: "SubCategories",
                newName: "IX_SubCategories_ParentCategoryId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SubCategories",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(32)",
                oldMaxLength: 32)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubCategories",
                table: "SubCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_SubCategories_SubCategoryId",
                table: "Products",
                column: "SubCategoryId",
                principalTable: "SubCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategories_Categories_ParentCategoryId",
                table: "SubCategories",
                column: "ParentCategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
