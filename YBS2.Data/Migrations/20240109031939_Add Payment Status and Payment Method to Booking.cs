using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YBS2.Data.Migrations
{
    public partial class AddPaymentStatusandPaymentMethodtoBooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentMethod",
                table: "Booking",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatus",
                table: "Booking",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                table: "Booking");
        }
    }
}
