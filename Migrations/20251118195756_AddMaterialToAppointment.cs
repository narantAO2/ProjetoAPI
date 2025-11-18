using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OdontoApi.Migrations
{
    /// <inheritdoc />
    public partial class AddMaterialToAppointment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaterialId",
                table: "Appointments",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_MaterialId",
                table: "Appointments",
                column: "MaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Materials_MaterialId",
                table: "Appointments",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Materials_MaterialId",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_MaterialId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "MaterialId",
                table: "Appointments");
        }
    }
}
