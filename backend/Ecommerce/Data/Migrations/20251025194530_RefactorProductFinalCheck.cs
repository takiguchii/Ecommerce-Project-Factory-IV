using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class RefactorProductFinalCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_brand_BrandId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_category_CategoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_provider_ProviderId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_sub_category_SubCategoryId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_BrandId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_SubCategoryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AdditionalImageUrl1",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AdditionalImageUrl2",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AdditionalImageUrl3",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AdditionalImageUrl4",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "AverageStars",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "BrandId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CoverImageUrl",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DiscountPrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OriginalPrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RawDescription",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "RawTechnicalInfo",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "TechnicalInfo",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "product");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "product",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "product",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "product",
                newName: "code");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "product",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "SubCategoryId",
                table: "product",
                newName: "rating");

            migrationBuilder.RenameColumn(
                name: "ReviewCount",
                table: "product",
                newName: "provider_id");

            migrationBuilder.RenameColumn(
                name: "ProviderId",
                table: "product",
                newName: "category_id");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "product",
                newName: "brand_id");

            migrationBuilder.RenameIndex(
                name: "IX_Products_ProviderId",
                table: "product",
                newName: "IX_product_category_id");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CategoryId",
                table: "product",
                newName: "IX_product_brand_id");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "brand",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(32)",
                oldMaxLength: 32)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "brand_image_url",
                table: "brand",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(512)",
                oldMaxLength: 512)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "product",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "product",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "code",
                table: "product",
                type: "varchar(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "discount_price",
                table: "product",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "image_url0",
                table: "product",
                type: "varchar(512)",
                maxLength: 512,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "image_url1",
                table: "product",
                type: "varchar(512)",
                maxLength: 512,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "image_url2",
                table: "product",
                type: "varchar(512)",
                maxLength: 512,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "image_url3",
                table: "product",
                type: "varchar(512)",
                maxLength: 512,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "image_url4",
                table: "product",
                type: "varchar(512)",
                maxLength: 512,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "original_price",
                table: "product",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "stars",
                table: "product",
                type: "decimal(3,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "sub_category_id",
                table: "product",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "technical_info",
                table: "product",
                type: "TEXT",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_product",
                table: "product",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_product_provider_id",
                table: "product",
                column: "provider_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_sub_category_id",
                table: "product",
                column: "sub_category_id");

            migrationBuilder.AddForeignKey(
                name: "FK_product_brand_brand_id",
                table: "product",
                column: "brand_id",
                principalTable: "brand",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_product_category_category_id",
                table: "product",
                column: "category_id",
                principalTable: "category",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_product_provider_provider_id",
                table: "product",
                column: "provider_id",
                principalTable: "provider",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_product_sub_category_sub_category_id",
                table: "product",
                column: "sub_category_id",
                principalTable: "sub_category",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_product_brand_brand_id",
                table: "product");

            migrationBuilder.DropForeignKey(
                name: "FK_product_category_category_id",
                table: "product");

            migrationBuilder.DropForeignKey(
                name: "FK_product_provider_provider_id",
                table: "product");

            migrationBuilder.DropForeignKey(
                name: "FK_product_sub_category_sub_category_id",
                table: "product");

            migrationBuilder.DropPrimaryKey(
                name: "PK_product",
                table: "product");

            migrationBuilder.DropIndex(
                name: "IX_product_provider_id",
                table: "product");

            migrationBuilder.DropIndex(
                name: "IX_product_sub_category_id",
                table: "product");

            migrationBuilder.DropColumn(
                name: "discount_price",
                table: "product");

            migrationBuilder.DropColumn(
                name: "image_url0",
                table: "product");

            migrationBuilder.DropColumn(
                name: "image_url1",
                table: "product");

            migrationBuilder.DropColumn(
                name: "image_url2",
                table: "product");

            migrationBuilder.DropColumn(
                name: "image_url3",
                table: "product");

            migrationBuilder.DropColumn(
                name: "image_url4",
                table: "product");

            migrationBuilder.DropColumn(
                name: "original_price",
                table: "product");

            migrationBuilder.DropColumn(
                name: "stars",
                table: "product");

            migrationBuilder.DropColumn(
                name: "sub_category_id",
                table: "product");

            migrationBuilder.DropColumn(
                name: "technical_info",
                table: "product");

            migrationBuilder.RenameTable(
                name: "product",
                newName: "Products");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Products",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Products",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "code",
                table: "Products",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Products",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "rating",
                table: "Products",
                newName: "SubCategoryId");

            migrationBuilder.RenameColumn(
                name: "provider_id",
                table: "Products",
                newName: "ReviewCount");

            migrationBuilder.RenameColumn(
                name: "category_id",
                table: "Products",
                newName: "ProviderId");

            migrationBuilder.RenameColumn(
                name: "brand_id",
                table: "Products",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_product_category_id",
                table: "Products",
                newName: "IX_Products_ProviderId");

            migrationBuilder.RenameIndex(
                name: "IX_product_brand_id",
                table: "Products",
                newName: "IX_Products_CategoryId");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "brand",
                type: "varchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "brand_image_url",
                table: "brand",
                type: "varchar(512)",
                maxLength: 512,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Products",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Products",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(16)",
                oldMaxLength: 16)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalImageUrl1",
                table: "Products",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalImageUrl2",
                table: "Products",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalImageUrl3",
                table: "Products",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalImageUrl4",
                table: "Products",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "AverageStars",
                table: "Products",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CoverImageUrl",
                table: "Products",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountPrice",
                table: "Products",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "OriginalPrice",
                table: "Products",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "RawDescription",
                table: "Products",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "RawTechnicalInfo",
                table: "Products",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "TechnicalInfo",
                table: "Products",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_SubCategoryId",
                table: "Products",
                column: "SubCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_brand_BrandId",
                table: "Products",
                column: "BrandId",
                principalTable: "brand",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_category_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "category",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_provider_ProviderId",
                table: "Products",
                column: "ProviderId",
                principalTable: "provider",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_sub_category_SubCategoryId",
                table: "Products",
                column: "SubCategoryId",
                principalTable: "sub_category",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
