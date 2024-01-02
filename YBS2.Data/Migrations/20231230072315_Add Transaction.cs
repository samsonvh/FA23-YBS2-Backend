using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YBS2.Data.Migrations
{
    public partial class AddTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MembershipRegistrationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    TotalAmount = table.Column<float>(type: "real", nullable: false),
                    BankCode = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    BankTranNo = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    CardType = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VNPayCode = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Success = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transaction_Booking_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Booking",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transaction_MembershipRegistration_MembershipRegistrationId",
                        column: x => x.MembershipRegistrationId,
                        principalTable: "MembershipRegistration",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_BookingId",
                table: "Transaction",
                column: "BookingId");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_MembershipRegistrationId",
                table: "Transaction",
                column: "MembershipRegistrationId",
                unique: true,
                filter: "[MembershipRegistrationId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transaction");
        }
    }
}
