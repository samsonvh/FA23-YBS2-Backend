using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YBS2.Data.Migrations
{
    public partial class Addsintofield : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalPassenger",
                table: "Yacht",
                newName: "TotalPassengers");

            migrationBuilder.RenameColumn(
                name: "TotalCrew",
                table: "Yacht",
                newName: "TotalCrews");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalPassengers",
                table: "Yacht",
                newName: "TotalPassenger");

            migrationBuilder.RenameColumn(
                name: "TotalCrews",
                table: "Yacht",
                newName: "TotalCrew");
        }
    }
}
