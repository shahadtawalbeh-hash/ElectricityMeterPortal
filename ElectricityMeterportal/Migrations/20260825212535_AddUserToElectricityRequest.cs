using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectricityMeterportal.Migrations
{
    /// <inheritdoc />
    public partial class AddUserToElectricityRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "ElectricityRequests",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ElectricityRequests");
        }
    }
}
