using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VMTS.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixfaultreportfk : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "FaultReports");

            migrationBuilder.AlterColumn<string>(
                name: "VehicleId",
                table: "FaultReports",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_FaultReports_VehicleId",
                table: "FaultReports",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_FaultReports_Vehicles_VehicleId",
                table: "FaultReports",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FaultReports_Vehicles_VehicleId",
                table: "FaultReports");

            migrationBuilder.DropIndex(
                name: "IX_FaultReports_VehicleId",
                table: "FaultReports");

            migrationBuilder.AlterColumn<string>(
                name: "VehicleId",
                table: "FaultReports",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "FaultReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
