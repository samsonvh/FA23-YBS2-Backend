using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YBS2.Data.Migrations
{
    public partial class Extenddescriptionintour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServicePackageItems_Service_ServiceId",
                table: "ServicePackageItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ServicePackageItems_ServicePackage_ServicePackageId",
                table: "ServicePackageItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServicePackageItems",
                table: "ServicePackageItems");

            migrationBuilder.RenameTable(
                name: "ServicePackageItems",
                newName: "ServicePackageItem");

            migrationBuilder.RenameIndex(
                name: "IX_ServicePackageItems_ServicePackageId",
                table: "ServicePackageItem",
                newName: "IX_ServicePackageItem_ServicePackageId");

            migrationBuilder.RenameIndex(
                name: "IX_ServicePackageItems_ServiceId",
                table: "ServicePackageItem",
                newName: "IX_ServicePackageItem_ServiceId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Tour",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServicePackageItem",
                table: "ServicePackageItem",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServicePackageItem_Service_ServiceId",
                table: "ServicePackageItem",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServicePackageItem_ServicePackage_ServicePackageId",
                table: "ServicePackageItem",
                column: "ServicePackageId",
                principalTable: "ServicePackage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServicePackageItem_Service_ServiceId",
                table: "ServicePackageItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ServicePackageItem_ServicePackage_ServicePackageId",
                table: "ServicePackageItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServicePackageItem",
                table: "ServicePackageItem");

            migrationBuilder.RenameTable(
                name: "ServicePackageItem",
                newName: "ServicePackageItems");

            migrationBuilder.RenameIndex(
                name: "IX_ServicePackageItem_ServicePackageId",
                table: "ServicePackageItems",
                newName: "IX_ServicePackageItems_ServicePackageId");

            migrationBuilder.RenameIndex(
                name: "IX_ServicePackageItem_ServiceId",
                table: "ServicePackageItems",
                newName: "IX_ServicePackageItems_ServiceId");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Tour",
                type: "nvarchar(500)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServicePackageItems",
                table: "ServicePackageItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServicePackageItems_Service_ServiceId",
                table: "ServicePackageItems",
                column: "ServiceId",
                principalTable: "Service",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServicePackageItems_ServicePackage_ServicePackageId",
                table: "ServicePackageItems",
                column: "ServicePackageId",
                principalTable: "ServicePackage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
