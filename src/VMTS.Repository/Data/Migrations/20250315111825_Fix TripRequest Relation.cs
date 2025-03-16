using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VMTS.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixTripRequestRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FaultType",
                table: "FaultReports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FaultType",
                table: "FaultReports");
        }
    }
}
