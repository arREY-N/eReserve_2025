using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class FacilityName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Floor",
                table: "Facilities");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Facilities");

            migrationBuilder.RenameColumn(
                name: "FacilityType",
                table: "Facilities",
                newName: "FacilityName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FacilityName",
                table: "Facilities",
                newName: "FacilityType");

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
                columns: new[] { "Floor", "Number" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "FacilityID",
                keyValue: 2,
                columns: new[] { "Floor", "Number" },
                values: new object[] { 6, 1 });

            migrationBuilder.UpdateData(
                table: "Facilities",
                keyColumn: "FacilityID",
                keyValue: 3,
                columns: new[] { "Floor", "Number" },
                values: new object[] { 9, 1 });
        }
    }
}
