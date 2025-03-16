using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VMTS.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixBusinessUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FaultReports_BusinessUser_DriverId",
                table: "FaultReports");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceReports_BusinessUser_MechanicId",
                table: "MaintenanceReports");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRequests_BusinessUser_MechanicId",
                table: "MaintenanceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TripsReports_BusinessUser_DriverId",
                table: "TripsReports");

            migrationBuilder.DropForeignKey(
                name: "FK_TripsRequests_BusinessUser_DriverId",
                table: "TripsRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TripsRequests_BusinessUser_ManagerId",
                table: "TripsRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BusinessUser",
                table: "BusinessUser");

            migrationBuilder.RenameTable(
                name: "BusinessUser",
                newName: "BusinessUsers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusinessUsers",
                table: "BusinessUsers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FaultReports_BusinessUsers_DriverId",
                table: "FaultReports",
                column: "DriverId",
                principalTable: "BusinessUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceReports_BusinessUsers_MechanicId",
                table: "MaintenanceReports",
                column: "MechanicId",
                principalTable: "BusinessUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRequests_BusinessUsers_MechanicId",
                table: "MaintenanceRequests",
                column: "MechanicId",
                principalTable: "BusinessUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TripsReports_BusinessUsers_DriverId",
                table: "TripsReports",
                column: "DriverId",
                principalTable: "BusinessUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TripsRequests_BusinessUsers_DriverId",
                table: "TripsRequests",
                column: "DriverId",
                principalTable: "BusinessUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TripsRequests_BusinessUsers_ManagerId",
                table: "TripsRequests",
                column: "ManagerId",
                principalTable: "BusinessUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FaultReports_BusinessUsers_DriverId",
                table: "FaultReports");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceReports_BusinessUsers_MechanicId",
                table: "MaintenanceReports");

            migrationBuilder.DropForeignKey(
                name: "FK_MaintenanceRequests_BusinessUsers_MechanicId",
                table: "MaintenanceRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TripsReports_BusinessUsers_DriverId",
                table: "TripsReports");

            migrationBuilder.DropForeignKey(
                name: "FK_TripsRequests_BusinessUsers_DriverId",
                table: "TripsRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_TripsRequests_BusinessUsers_ManagerId",
                table: "TripsRequests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BusinessUsers",
                table: "BusinessUsers");

            migrationBuilder.RenameTable(
                name: "BusinessUsers",
                newName: "BusinessUser");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusinessUser",
                table: "BusinessUser",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FaultReports_BusinessUser_DriverId",
                table: "FaultReports",
                column: "DriverId",
                principalTable: "BusinessUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceReports_BusinessUser_MechanicId",
                table: "MaintenanceReports",
                column: "MechanicId",
                principalTable: "BusinessUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaintenanceRequests_BusinessUser_MechanicId",
                table: "MaintenanceRequests",
                column: "MechanicId",
                principalTable: "BusinessUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TripsReports_BusinessUser_DriverId",
                table: "TripsReports",
                column: "DriverId",
                principalTable: "BusinessUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TripsRequests_BusinessUser_DriverId",
                table: "TripsRequests",
                column: "DriverId",
                principalTable: "BusinessUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TripsRequests_BusinessUser_ManagerId",
                table: "TripsRequests",
                column: "ManagerId",
                principalTable: "BusinessUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
