using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YBS2.Data.Migrations
{
    public partial class AddPhoneNumberEmailAddressSpecialRequestintopassenger : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Passenger",
                type: "nvarchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Passenger",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsLeader",
                table: "Passenger",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Passenger",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SpecialRequest",
                table: "Passenger",
                type: "nvarchar(500)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Passenger");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Passenger");

            migrationBuilder.DropColumn(
                name: "IsLeader",
                table: "Passenger");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Passenger");

            migrationBuilder.DropColumn(
                name: "SpecialRequest",
                table: "Passenger");
        }
    }
}
