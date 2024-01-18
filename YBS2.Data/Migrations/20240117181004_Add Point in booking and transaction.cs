using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YBS2.Data.Migrations
{
    public partial class AddPointinbookingandtransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Yacht",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "Point",
                table: "Transaction",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Point",
                table: "Booking",
                type: "real",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Yacht");

            migrationBuilder.DropColumn(
                name: "Point",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "Point",
                table: "Booking");
        }
    }
}
