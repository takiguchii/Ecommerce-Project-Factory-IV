using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ecommerce.Migrations
{
    /// <inheritdoc />
    public partial class RefactorProviderFinalCheck : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Providers_ProviderId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Providers",
                table: "Providers");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Providers");

            migrationBuilder.RenameTable(
                name: "Providers",
                newName: "provider");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "provider",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "provider",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Cnpj",
                table: "provider",
                newName: "cnpj");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "provider",
                newName: "address");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "provider",
                newName: "id");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "provider",
                type: "varchar(32)",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(256)",
                oldMaxLength: 256)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "cnpj",
                table: "provider",
                type: "varchar(16)",
                maxLength: 16,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(14)",
                oldMaxLength: 14)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "address",
                table: "provider",
                type: "varchar(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(256)",
                oldMaxLength: 256)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "phone_number",
                table: "provider",
                type: "varchar(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_provider",
                table: "provider",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_provider_ProviderId",
                table: "Products",
                column: "ProviderId",
                principalTable: "provider",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_provider_ProviderId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_provider",
                table: "provider");

            migrationBuilder.DropColumn(
                name: "phone_number",
                table: "provider");

            migrationBuilder.RenameTable(
                name: "provider",
                newName: "Providers");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Providers",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Providers",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "cnpj",
                table: "Providers",
                newName: "Cnpj");

            migrationBuilder.RenameColumn(
                name: "address",
                table: "Providers",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Providers",
                newName: "Id");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Providers",
                type: "varchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(32)",
                oldMaxLength: 32)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Cnpj",
                table: "Providers",
                type: "varchar(14)",
                maxLength: 14,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(16)",
                oldMaxLength: 16)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Providers",
                type: "varchar(256)",
                maxLength: 256,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(128)",
                oldMaxLength: 128)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Providers",
                type: "varchar(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Providers",
                table: "Providers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Providers_ProviderId",
                table: "Products",
                column: "ProviderId",
                principalTable: "Providers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
