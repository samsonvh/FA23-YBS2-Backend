using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YBS2.Data.Migrations
{
    public partial class Addmembershippackageidnullableinmembershipregistration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembershipRegistration_MembershipPackage_MembershipPackageId",
                table: "MembershipRegistration");

            migrationBuilder.AlterColumn<Guid>(
                name: "MembershipPackageId",
                table: "MembershipRegistration",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipRegistration_MembershipPackage_MembershipPackageId",
                table: "MembershipRegistration",
                column: "MembershipPackageId",
                principalTable: "MembershipPackage",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MembershipRegistration_MembershipPackage_MembershipPackageId",
                table: "MembershipRegistration");

            migrationBuilder.AlterColumn<Guid>(
                name: "MembershipPackageId",
                table: "MembershipRegistration",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MembershipRegistration_MembershipPackage_MembershipPackageId",
                table: "MembershipRegistration",
                column: "MembershipPackageId",
                principalTable: "MembershipPackage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
