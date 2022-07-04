using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class PrimeraMigracion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "Clasificaciones",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LegacyId = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAlta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clasificaciones", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Materiales",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Codigo = table.Column<int>(type: "int", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Costo = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ClasificacionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdTipoMaterial = table.Column<int>(type: "int", nullable: true),
                    LegacyId = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAlta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Materiales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Materiales_Clasificaciones_ClasificacionId",
                        column: x => x.ClasificacionId,
                        principalSchema: "dbo",
                        principalTable: "Clasificaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Materiales_ClasificacionId",
                schema: "dbo",
                table: "Materiales",
                column: "ClasificacionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Materiales",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Clasificaciones",
                schema: "dbo");
        }
    }
}
