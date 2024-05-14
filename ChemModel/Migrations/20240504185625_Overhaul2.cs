using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChemModel.Migrations
{
    /// <inheritdoc />
    public partial class Overhaul2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialMathModelPropBinds");

            migrationBuilder.DropTable(
                name: "MathModelEmpiricBinds");

            migrationBuilder.DropTable(
                name: "VarCoefficients");

            migrationBuilder.DropTable(
                name: "MaterialMathModelProperties");

            migrationBuilder.CreateTable(
                name: "EmpiricCoefficientMaths",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PropertyId = table.Column<int>(type: "INTEGER", nullable: false),
                    MathModelId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmpiricCoefficientMaths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmpiricCoefficientMaths_EmpiricCoefficients_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "EmpiricCoefficients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmpiricCoefficientMaths_MathModels_MathModelId",
                        column: x => x.MathModelId,
                        principalTable: "MathModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialEmpiricBinds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PropertyId = table.Column<int>(type: "INTEGER", nullable: false),
                    MaterialId = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialEmpiricBinds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialEmpiricBinds_EmpiricCoefficients_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "EmpiricCoefficients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialEmpiricBinds_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAddMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    MaterialId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAddMaterials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAddMaterials_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAddMaterials_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAddMathModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    MathModelId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAddMathModels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAddMathModels_MathModels_MathModelId",
                        column: x => x.MathModelId,
                        principalTable: "MathModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAddMathModels_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VarCoefficientsMaths",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PropertyId = table.Column<int>(type: "INTEGER", nullable: false),
                    MathModelId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VarCoefficientsMaths", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VarCoefficientsMaths_MathModels_MathModelId",
                        column: x => x.MathModelId,
                        principalTable: "MathModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VarCoefficientsMaths_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmpiricCoefficientMaths_MathModelId",
                table: "EmpiricCoefficientMaths",
                column: "MathModelId");

            migrationBuilder.CreateIndex(
                name: "IX_EmpiricCoefficientMaths_PropertyId",
                table: "EmpiricCoefficientMaths",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialEmpiricBinds_MaterialId",
                table: "MaterialEmpiricBinds",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialEmpiricBinds_PropertyId",
                table: "MaterialEmpiricBinds",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddMaterials_MaterialId",
                table: "UserAddMaterials",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddMaterials_UserId",
                table: "UserAddMaterials",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddMathModels_MathModelId",
                table: "UserAddMathModels",
                column: "MathModelId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAddMathModels_UserId",
                table: "UserAddMathModels",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_VarCoefficientsMaths_MathModelId",
                table: "VarCoefficientsMaths",
                column: "MathModelId");

            migrationBuilder.CreateIndex(
                name: "IX_VarCoefficientsMaths_PropertyId",
                table: "VarCoefficientsMaths",
                column: "PropertyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmpiricCoefficientMaths");

            migrationBuilder.DropTable(
                name: "MaterialEmpiricBinds");

            migrationBuilder.DropTable(
                name: "UserAddMaterials");

            migrationBuilder.DropTable(
                name: "UserAddMathModels");

            migrationBuilder.DropTable(
                name: "VarCoefficientsMaths");

            migrationBuilder.CreateTable(
                name: "MaterialMathModelProperties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UnitsId = table.Column<int>(type: "INTEGER", nullable: false),
                    Chars = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialMathModelProperties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialMathModelProperties_Units_UnitsId",
                        column: x => x.UnitsId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MathModelEmpiricBinds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MathModelId = table.Column<int>(type: "INTEGER", nullable: false),
                    PropertyId = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MathModelEmpiricBinds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MathModelEmpiricBinds_EmpiricCoefficients_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "EmpiricCoefficients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MathModelEmpiricBinds_MathModels_MathModelId",
                        column: x => x.MathModelId,
                        principalTable: "MathModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VarCoefficients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MathModelId = table.Column<int>(type: "INTEGER", nullable: false),
                    PropertyId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VarCoefficients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VarCoefficients_MathModels_MathModelId",
                        column: x => x.MathModelId,
                        principalTable: "MathModels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VarCoefficients_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaterialMathModelPropBinds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MaterialId = table.Column<int>(type: "INTEGER", nullable: false),
                    PropertyId = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<double>(type: "REAL", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialMathModelPropBinds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaterialMathModelPropBinds_MaterialMathModelProperties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "MaterialMathModelProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MaterialMathModelPropBinds_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaterialMathModelPropBinds_MaterialId",
                table: "MaterialMathModelPropBinds",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialMathModelPropBinds_PropertyId",
                table: "MaterialMathModelPropBinds",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_MaterialMathModelProperties_UnitsId",
                table: "MaterialMathModelProperties",
                column: "UnitsId");

            migrationBuilder.CreateIndex(
                name: "IX_MathModelEmpiricBinds_MathModelId",
                table: "MathModelEmpiricBinds",
                column: "MathModelId");

            migrationBuilder.CreateIndex(
                name: "IX_MathModelEmpiricBinds_PropertyId",
                table: "MathModelEmpiricBinds",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_VarCoefficients_MathModelId",
                table: "VarCoefficients",
                column: "MathModelId");

            migrationBuilder.CreateIndex(
                name: "IX_VarCoefficients_PropertyId",
                table: "VarCoefficients",
                column: "PropertyId");
        }
    }
}
