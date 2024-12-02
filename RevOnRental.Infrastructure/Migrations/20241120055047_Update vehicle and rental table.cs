using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RevOnRental.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Updatevehicleandrentaltable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfAvailableVehicle",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "RentalStatus",
                table: "Rentals",
                type: "int",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<decimal>(
                name: "AverageRating",
                table: "Businesses",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "TotalRating",
                table: "Businesses",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfAvailableVehicle",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "AverageRating",
                table: "Businesses");

            migrationBuilder.DropColumn(
                name: "TotalRating",
                table: "Businesses");

            migrationBuilder.AlterColumn<string>(
                name: "RentalStatus",
                table: "Rentals",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 50);
        }
    }
}
