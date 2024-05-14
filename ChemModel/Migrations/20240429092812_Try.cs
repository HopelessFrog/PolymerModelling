using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChemModel.Migrations
{
    /// <inheritdoc />
    public partial class Try : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Units_UnitsId",
                table: "Properties");

            migrationBuilder.RenameColumn(
                name: "UnitsId",
                table: "Properties",
                newName: "UnitId");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_UnitsId",
                table: "Properties",
                newName: "IX_Properties_UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Units_UnitId",
                table: "Properties",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Units_UnitId",
                table: "Properties");

            migrationBuilder.RenameColumn(
                name: "UnitId",
                table: "Properties",
                newName: "UnitsId");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_UnitId",
                table: "Properties",
                newName: "IX_Properties_UnitsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Units_UnitsId",
                table: "Properties",
                column: "UnitsId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
