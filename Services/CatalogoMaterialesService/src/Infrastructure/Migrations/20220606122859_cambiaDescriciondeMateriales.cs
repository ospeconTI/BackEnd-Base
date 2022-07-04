using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class cambiaDescriciondeMateriales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nombre",
                schema: "dbo",
                table: "Materiales",
                newName: "Descripcion");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Descripcion",
                schema: "dbo",
                table: "Materiales",
                newName: "Nombre");
        }
    }
}
