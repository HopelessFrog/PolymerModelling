using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChemModel.Migrations
{
    /// <inheritdoc />
    public partial class Making : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmpiricCoefficientMaths");

            migrationBuilder.DropTable(
                name: "UserAddMathModels");

            migrationBuilder.DropTable(
                name: "VarCoefficientsMaths");

            migrationBuilder.DropTable(
                name: "MathModels");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MathModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Formula = table.Column<string>(type: "varchar(200)", nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false),
                    TexFormula = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MathModels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmpiricCoefficientMaths",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MathModelId = table.Column<int>(type: "INTEGER", nullable: false),
                    PropertyId = table.Column<int>(type: "INTEGER", nullable: false)
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
                name: "UserAddMathModels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MathModelId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
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
                    MathModelId = table.Column<int>(type: "INTEGER", nullable: false),
                    PropertyId = table.Column<int>(type: "INTEGER", nullable: false)
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
    }
}
