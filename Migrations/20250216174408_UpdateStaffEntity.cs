using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eTutoring.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStaffEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Staffs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "Staffs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Staffs");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "Staffs");
        }
    }
}
