using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VMTS.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class editfaultreport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "FaultReports");

            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "FaultReports");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "FaultReports");

            migrationBuilder.AddColumn<string>(
                name: "FaultAddress",
                table: "FaultReports",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FaultAddress",
                table: "FaultReports");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "FaultReports",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "FaultReports",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "FaultReports",
                type: "float",
                nullable: true);
        }
    }
}
