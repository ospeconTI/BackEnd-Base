
namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.Queries
{
    using Dapper;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using System.Linq;
    using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Domain.Entities;
    using OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.Exceptions;
    using Newtonsoft.Json.Linq;

    public class MaterialesQueries
        : IMaterialesQueries
    {
        private string _connectionString = string.Empty;

        public MaterialesQueries(string constr)
        {
            _connectionString = !string.IsNullOrWhiteSpace(constr) ? constr : throw new ArgumentNullException(nameof(constr));
        }


        public async Task<MaterialesDTO> GetMaterialesAsync(int codigo)

        {

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sql = @"SELECT m.Id, m.Codigo, m.Descripcion, m.Costo,  
                c.Id, c.Descripcion as Descripcion, tm.Id, tm.Descripcion
                    FROM     dbo.Materiales m Left join dbo.Clasificaciones c on m.ClasificacionId=c.Id 
                        left join dbo.TipoMateriales tm on tm.id = m.TipoMaterialId
                    where m.codigo = @codigo;";

                IEnumerable<MaterialesDTO> multiple = await connection.QueryAsync<MaterialesDTO, ClasificacionDTO, tipoMaterialDTO, MaterialesDTO>(sql, (material, clasif_materiales, tipoMaterialDTO) =>
                {

                    material.Clasificacion = clasif_materiales ?? new ClasificacionDTO();
                    material.TipoMaterial = tipoMaterialDTO ?? new tipoMaterialDTO();
                    return material;
                },
                param: new { codigo });

                if (multiple.Count() == 0) throw new NotFoundException();

                return multiple.First();
            }
        }


        public async Task<IEnumerable<MaterialesDTO>> GetMaterialesByNameAsync(string descripcion)

        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sql = @"SELECT m.Id, m.Codigo, m.Descripcion, m.Costo, m.ClasificacionId, m.TipoMaterialID, 
                isNull(c.Descripcion,'SIN CLASIFICACION') as clasifDescripcion, 
                isNull(tm.Descripcion, 'SIN TIPO DE MATERIAL') as TipoMaterialDescripcion, 
                c.Id, c.Descripcion as Descripcion, tm.Id, tm.Descripcion
                    FROM     dbo.Materiales m Left join dbo.Clasificaciones c on m.ClasificacionId=c.Id 
                        left join dbo.TipoMateriales tm on tm.id = m.TipoMaterialId
                    where rtrim(ltrim(m.Descripcion)) like '%' + @descripcion + '%' Order by m.Descripcion;";

                var material = await connection.QueryAsync<MaterialesDTO, ClasificacionDTO, tipoMaterialDTO, MaterialesDTO>(sql, (material, clasif_materiales, tipoMaterialDTO) =>
                {
                    material.Clasificacion = clasif_materiales ?? new ClasificacionDTO();
                    material.TipoMaterial = tipoMaterialDTO ?? new tipoMaterialDTO();

                    return material;
                },
                param: new { descripcion });

                if (material == null)
                    throw new KeyNotFoundException();

                return material;
            }
        }

        public async Task<IEnumerable<MaterialesDTO>> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sql = @"SELECT m.Id, m.Codigo, m.Descripcion, m.Costo, m.ClasificacionId, m.TipoMaterialID, 
                isNull(c.Descripcion,'SIN CLASIFICACION') as clasifDescripcion, 
                isNull(tm.Descripcion, 'SIN TIPO DE MATERIAL') as TipoMaterialDescripcion, 
                c.Id, c.Descripcion as Descripcion, tm.Id, tm.Descripcion
                    FROM     dbo.Materiales m Left join dbo.Clasificaciones c on m.ClasificacionId=c.Id 
                        left join dbo.TipoMateriales tm on tm.id = m.TipoMaterialId
                    order by m.Descripcion";

                var material = await connection.QueryAsync<MaterialesDTO, ClasificacionDTO, tipoMaterialDTO, MaterialesDTO>(sql, (material, clasif_materiales, tipoMaterialDTO) =>
                {
                    material.Clasificacion = clasif_materiales ?? new ClasificacionDTO();
                    material.TipoMaterial = tipoMaterialDTO ?? new tipoMaterialDTO();
                    return material;
                });

                return material;
            }
        }

        public async Task<IEnumerable<MaterialesDTO>> GetMaterialeByDescripcionesCombinadasAsync(string descripcion)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var sql = @"SELECT m.Id, m.Codigo, m.Descripcion, m.Costo, m.ClasificacionId, m.TipoMaterialID, 
                isNull(c.Descripcion,'SIN CLASIFICACION') as clasifDescripcion, 
                isNull(tm.Descripcion, 'SIN TIPO DE MATERIAL') as TipoMaterialDescripcion, 
                c.Id, c.Descripcion as Descripcion, tm.Id, tm.Descripcion
                    FROM     dbo.Materiales m Left join dbo.Clasificaciones c on m.ClasificacionId=c.Id 
                        left join dbo.TipoMateriales tm on tm.id = m.TipoMaterialId
                    where cast(Codigo as varchar(8)) + rtrim(ltrim(m.Descripcion)) + isNull(rtrim(ltrim(c.Descripcion)), 'SIN CLASIFICACION') like '%' + @descripcion + '%' Order by m.Descripcion;";

                var material = await connection.QueryAsync<MaterialesDTO, ClasificacionDTO, tipoMaterialDTO, MaterialesDTO>(sql, (material, clasif_materiales, tipoMaterialDTO) =>
                {
                    material.Clasificacion = clasif_materiales ?? new ClasificacionDTO();
                    material.TipoMaterial = tipoMaterialDTO ?? new tipoMaterialDTO();

                    return material;
                },
                param: new { descripcion });

                if (material == null)
                    throw new KeyNotFoundException();

                return material;
            }

        }


    }

}