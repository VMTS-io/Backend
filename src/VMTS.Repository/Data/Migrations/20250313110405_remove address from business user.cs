using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VMTS.Repository.Data.Migrations
{
    /// <inheritdoc />
    public partial class removeaddressfrombusinessuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address_AppUserId",
                table: "BusinessUsers");

            migrationBuilder.DropColumn(
                name: "Address_Area",
                table: "BusinessUsers");

            migrationBuilder.DropColumn(
                name: "Address_Country",
                table: "BusinessUsers");

            migrationBuilder.DropColumn(
                name: "Address_Governorate",
                table: "BusinessUsers");

            migrationBuilder.DropColumn(
                name: "Address_Id",
                table: "BusinessUsers");

            migrationBuilder.DropColumn(
                name: "Address_PostalCode",
                table: "BusinessUsers");

            migrationBuilder.DropColumn(
                name: "Address_Street",
                table: "BusinessUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address_AppUserId",
                table: "BusinessUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Area",
                table: "BusinessUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Country",
                table: "BusinessUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Governorate",
                table: "BusinessUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Address_Id",
                table: "BusinessUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_PostalCode",
                table: "BusinessUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address_Street",
                table: "BusinessUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
