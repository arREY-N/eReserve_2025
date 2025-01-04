using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class EditReservations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.RenameColumn(
				name: "Id",
				table: "Reservations",
				newName: "RequestID");

			migrationBuilder.RenameColumn(
				name: "ReserveDateTime",
				table: "Reservations",
				newName: "ReserveDate");

			migrationBuilder.AddColumn<TimeSpan>(
				name: "ReserveTimeEnd",
				table: "Reservations",
				type: "time",
				nullable: false,
				defaultValue: new TimeSpan(0, 0, 0, 0, 0));

			migrationBuilder.AddColumn<TimeSpan>(
				name: "ReserveTimeStart",
				table: "Reservations",
				type: "time",
				nullable: false,
				defaultValue: new TimeSpan(0, 0, 0, 0, 0));
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
