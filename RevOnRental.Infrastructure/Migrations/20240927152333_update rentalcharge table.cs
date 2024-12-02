using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RevOnRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updaterentalchargetable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RentalCharge_VehicleId",
                table: "RentalCharge");

            migrationBuilder.DropColumn(
                name: "TimeEnum",
                table: "RentalCharge");

            migrationBuilder.RenameColumn(
                name: "Rate",
                table: "RentalCharge",
                newName: "HourlyRate");

            migrationBuilder.AddColumn<double>(
                name: "FullDayRate",
                table: "RentalCharge",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "HalfDayRate",
                table: "RentalCharge",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_RentalCharge_VehicleId",
                table: "RentalCharge",
                column: "VehicleId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RentalCharge_VehicleId",
                table: "RentalCharge");

            migrationBuilder.DropColumn(
                name: "FullDayRate",
                table: "RentalCharge");

            migrationBuilder.DropColumn(
                name: "HalfDayRate",
                table: "RentalCharge");

            migrationBuilder.RenameColumn(
                name: "HourlyRate",
                table: "RentalCharge",
                newName: "Rate");

            migrationBuilder.AddColumn<int>(
                name: "TimeEnum",
                table: "RentalCharge",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RentalCharge_VehicleId",
                table: "RentalCharge",
                column: "VehicleId");
        }
    }
}
