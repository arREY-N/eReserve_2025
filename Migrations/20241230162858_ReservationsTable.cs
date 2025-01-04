using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class ReservationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.CreateTable(
				name: "Reservations",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					ReserveDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
					RequestDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
					FacilityID = table.Column<int>(type: "int", nullable: false),
					RoomNumber = table.Column<int>(type: "int", nullable: false),
					Purpose = table.Column<string>(type: "nvarchar(max)", nullable: true),
					UserID = table.Column<int>(type: "int", nullable: false),
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Reservations", x => x.Id);
				});

			migrationBuilder.AddForeignKey(
				name: "FK_Reservations_Users_UserID",
				table: "Reservations",
				column: "UserID",
				principalTable: "Users",
				principalColumn: "UserID",
				onDelete: ReferentialAction.Cascade);
			
			migrationBuilder.AddForeignKey(
					name: "FK_Reservations_Facilities_FacilityID",
					table: "Reservations",
					column: "FacilityID",
					principalTable: "Facilities",
					principalColumn: "FacilityID",
					onDelete: ReferentialAction.Cascade);

			migrationBuilder.CreateIndex(
				name: "IX_Reservations_UserID",
				table: "Reservations",
				column: "UserID");

			migrationBuilder.CreateIndex(
				name: "IX_Reservations_FacilityID",
				table: "Reservations",
				column: "FacilityID");
		}

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
