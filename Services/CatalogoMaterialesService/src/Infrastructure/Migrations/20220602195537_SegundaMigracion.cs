using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class SegundaMigracion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TipoMateriales",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TipoMaterialesId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LegacyId = table.Column<int>(type: "int", nullable: false),
                    Activo = table.Column<bool>(type: "bit", nullable: false),
                    FechaAlta = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioAlta = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FechaUpdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UsuarioUpdate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoMateriales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TipoMateriales_TipoMateriales_TipoMaterialesId",
                        column: x => x.TipoMaterialesId,
                        principalSchema: "dbo",
                        principalTable: "TipoMateriales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TipoMateriales_TipoMaterialesId",
                schema: "dbo",
                table: "TipoMateriales",
                column: "TipoMaterialesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TipoMateriales",
                schema: "dbo");
        }
    }
}
