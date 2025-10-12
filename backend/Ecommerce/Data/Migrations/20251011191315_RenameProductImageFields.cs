using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class RenameProductImageFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl4",
                table: "Products",
                newName: "CoverImageUrl");

            migrationBuilder.RenameColumn(
                name: "ImageUrl3",
                table: "Products",
                newName: "AdditionalImageUrl3");

            migrationBuilder.RenameColumn(
                name: "ImageUrl2",
                table: "Products",
                newName: "AdditionalImageUrl2");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Products",
                newName: "AdditionalImageUrl1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CoverImageUrl",
                table: "Products",
                newName: "ImageUrl4");

            migrationBuilder.RenameColumn(
                name: "AdditionalImageUrl3",
                table: "Products",
                newName: "ImageUrl3");

            migrationBuilder.RenameColumn(
                name: "AdditionalImageUrl2",
                table: "Products",
                newName: "ImageUrl2");

            migrationBuilder.RenameColumn(
                name: "AdditionalImageUrl1",
                table: "Products",
                newName: "ImageUrl");
        }
    }
}
