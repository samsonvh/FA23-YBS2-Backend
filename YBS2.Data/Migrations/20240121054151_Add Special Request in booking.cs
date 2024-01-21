using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YBS2.Data.Migrations
{
    public partial class AddSpecialRequestinbooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpecialRequest",
                table: "Passenger");

            migrationBuilder.AddColumn<string>(
                name: "SpecialRequest",
                table: "Booking",
                type: "nvarchar(500)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SpecialRequest",
                table: "Booking");

            migrationBuilder.AddColumn<string>(
                name: "SpecialRequest",
                table: "Passenger",
                type: "nvarchar(500)",
                nullable: true);
        }
    }
}
