using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RevOnRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updateDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalCharge_Vehicles_VehicleId",
                table: "RentalCharge");

            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Vehicles_VehicleId",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Businesses_BusinessID",
                table: "Vehicles");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalCharge_Vehicles_VehicleId",
                table: "RentalCharge",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Vehicles_VehicleId",
                table: "Rentals",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Businesses_BusinessID",
                table: "Vehicles",
                column: "BusinessID",
                principalTable: "Businesses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RentalCharge_Vehicles_VehicleId",
                table: "RentalCharge");

            migrationBuilder.DropForeignKey(
                name: "FK_Rentals_Vehicles_VehicleId",
                table: "Rentals");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Businesses_BusinessID",
                table: "Vehicles");

            migrationBuilder.AddForeignKey(
                name: "FK_RentalCharge_Vehicles_VehicleId",
                table: "RentalCharge",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rentals_Vehicles_VehicleId",
                table: "Rentals",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Businesses_BusinessID",
                table: "Vehicles",
                column: "BusinessID",
                principalTable: "Businesses",
                principalColumn: "Id");
        }
    }
}
