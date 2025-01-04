using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class FacilityFloor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Floor",
                table: "Facilities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Number",
                table: "Facilities",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "FacilityID",
                keyValue: 1,
                columns: new[] { "FacilityName", "Floor", "Number" },
                values: new object[] { "RM101", 1, 1 });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "FacilityID",
                keyValue: 2,
                columns: new[] { "FacilityName", "Floor", "Number" },
                values: new object[] { "RM601", 6, 1 });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "FacilityID",
                keyValue: 3,
                columns: new[] { "Floor", "Number" },
                values: new object[] { 9, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Floor",
                table: "Facilities");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Facilities");

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "FacilityID",
                keyValue: 1,
                column: "FacilityName",
                value: "Room101");

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "FacilityID",
                keyValue: 2,
                column: "FacilityName",
                value: "Lab601");
        }
    }
}
