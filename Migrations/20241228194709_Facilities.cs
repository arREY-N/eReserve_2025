using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class Facilities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserReservations_Reservations_ReservationRequestID",
                table: "UserReservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserReservations",
                table: "UserReservations");

            migrationBuilder.DropIndex(
                name: "IX_UserReservations_UserID",
                table: "UserReservations");

            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "UserReservations");

            migrationBuilder.RenameColumn(
                name: "ReservationRequestID",
                table: "UserReservations",
                newName: "ReservationID");

            migrationBuilder.RenameIndex(
                name: "IX_UserReservations_ReservationRequestID",
                table: "UserReservations",
                newName: "IX_UserReservations_ReservationID");

            migrationBuilder.RenameColumn(
                name: "RoomNumber",
                table: "Reservations",
                newName: "FacilityID");

            migrationBuilder.RenameColumn(
                name: "FacityID",
                table: "Facilities",
                newName: "FacilityID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserReservations",
                table: "UserReservations",
                columns: new[] { "UserID", "ReservationID" });

            migrationBuilder.InsertData(
                table: "Facilities",
                columns: new[] { "FacilityID", "FacilityType", "Floor", "Number" },
                values: new object[,]
                {
                    { 1, "Room101", 1, 1 },
                    { 2, "Lab601", 6, 1 },
                    { 3, "Penthouse", 9, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_FacilityID",
                table: "Reservations",
                column: "FacilityID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Facilities_FacilityID",
                table: "Reservations",
                column: "FacilityID",
                principalTable: "Facilities",
                principalColumn: "FacilityID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserReservations_Reservations_ReservationID",
                table: "UserReservations",
                column: "ReservationID",
                principalTable: "Reservations",
                principalColumn: "RequestID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Facilities_FacilityID",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_UserReservations_Reservations_ReservationID",
                table: "UserReservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserReservations",
                table: "UserReservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_FacilityID",
                table: "Reservations");

            migrationBuilder.DeleteData(
                table: "Facilities",
                keyColumn: "FacilityID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Facilities",
                keyColumn: "FacilityID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Facilities",
                keyColumn: "FacilityID",
                keyValue: 3);

            migrationBuilder.RenameColumn(
                name: "ReservationID",
                table: "UserReservations",
                newName: "ReservationRequestID");

            migrationBuilder.RenameIndex(
                name: "IX_UserReservations_ReservationID",
                table: "UserReservations",
                newName: "IX_UserReservations_ReservationRequestID");

            migrationBuilder.RenameColumn(
                name: "FacilityID",
                table: "Reservations",
                newName: "RoomNumber");

            migrationBuilder.RenameColumn(
                name: "FacilityID",
                table: "Facilities",
                newName: "FacityID");

            migrationBuilder.AddColumn<int>(
                name: "RequestId",
                table: "UserReservations",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserReservations",
                table: "UserReservations",
                column: "RequestId");

            migrationBuilder.CreateIndex(
                name: "IX_UserReservations_UserID",
                table: "UserReservations",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserReservations_Reservations_ReservationRequestID",
                table: "UserReservations",
                column: "ReservationRequestID",
                principalTable: "Reservations",
                principalColumn: "RequestID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
