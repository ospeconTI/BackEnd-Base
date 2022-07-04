using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Migrations
{
    public partial class arreglaFKTipoMateriales : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TipoMateriales_TipoMateriales_TipoMaterialesId",
                schema: "dbo",
                table: "TipoMateriales");

            migrationBuilder.DropIndex(
                name: "IX_TipoMateriales_TipoMaterialesId",
                schema: "dbo",
                table: "TipoMateriales");

            migrationBuilder.DropColumn(
                name: "TipoMaterialesId",
                schema: "dbo",
                table: "TipoMateriales");

            migrationBuilder.DropColumn(
                name: "IdTipoMaterial",
                schema: "dbo",
                table: "Materiales");

            migrationBuilder.AddColumn<Guid>(
                name: "TipoMaterialId",
                schema: "dbo",
                table: "Materiales",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materiales_TipoMaterialId",
                schema: "dbo",
                table: "Materiales",
                column: "TipoMaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materiales_TipoMateriales_TipoMaterialId",
                schema: "dbo",
                table: "Materiales",
                column: "TipoMaterialId",
                principalSchema: "dbo",
                principalTable: "TipoMateriales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materiales_TipoMateriales_TipoMaterialId",
                schema: "dbo",
                table: "Materiales");

            migrationBuilder.DropIndex(
                name: "IX_Materiales_TipoMaterialId",
                schema: "dbo",
                table: "Materiales");

            migrationBuilder.DropColumn(
                name: "TipoMaterialId",
                schema: "dbo",
                table: "Materiales");

            migrationBuilder.AddColumn<Guid>(
                name: "TipoMaterialesId",
                schema: "dbo",
                table: "TipoMateriales",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdTipoMaterial",
                schema: "dbo",
                table: "Materiales",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TipoMateriales_TipoMaterialesId",
                schema: "dbo",
                table: "TipoMateriales",
                column: "TipoMaterialesId");

            migrationBuilder.AddForeignKey(
                name: "FK_TipoMateriales_TipoMateriales_TipoMaterialesId",
                schema: "dbo",
                table: "TipoMateriales",
                column: "TipoMaterialesId",
                principalSchema: "dbo",
                principalTable: "TipoMateriales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
