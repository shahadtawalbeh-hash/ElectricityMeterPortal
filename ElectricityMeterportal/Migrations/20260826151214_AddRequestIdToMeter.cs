using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElectricityMeterportal.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestIdToMeter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequestId",
                table: "Meters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Meters_RequestId",
                table: "Meters",
                column: "RequestId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Meters_ElectricityRequests_RequestId",
                table: "Meters",
                column: "RequestId",
                principalTable: "ElectricityRequests",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meters_ElectricityRequests_RequestId",
                table: "Meters");

            migrationBuilder.DropIndex(
                name: "IX_Meters_RequestId",
                table: "Meters");

            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "Meters");
        }
    }
}
