using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YBS2.Data.Migrations
{
    public partial class Changedurationunitoftourandmembershippackagetoenum1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "DurationUnit",
                table: "Tour",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)");

            migrationBuilder.AlterColumn<int>(
                name: "DurationUnit",
                table: "MembershipPackage",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DurationUnit",
                table: "Tour",
                type: "nvarchar(10)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "DurationUnit",
                table: "MembershipPackage",
                type: "nvarchar(10)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
