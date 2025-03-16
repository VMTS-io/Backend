using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VMTS.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToFaultReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "FaultReports",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "FaultReports");
        }
    }
}
