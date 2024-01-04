using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YBS2.Data.Migrations
{
    public partial class AddTourDock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TourDock",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TourId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DockId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourDock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourDock_Dock_DockId",
                        column: x => x.DockId,
                        principalTable: "Dock",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TourDock_Tour_TourId",
                        column: x => x.TourId,
                        principalTable: "Tour",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TourDock_DockId",
                table: "TourDock",
                column: "DockId");

            migrationBuilder.CreateIndex(
                name: "IX_TourDock_TourId",
                table: "TourDock",
                column: "TourId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TourDock");
        }
    }
}
