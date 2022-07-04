
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

    public class TipoMaterialesQueries
        : ITipoMaterialesQueries
    {
        private string _connectionString = string.Empty;

        public TipoMaterialesQueries(string constr)
        {
            _connectionString = !string.IsNullOrWhiteSpace(constr) ? constr : throw new ArgumentNullException(nameof(constr));
        }

        public async Task<tipoMaterialDTO> GetTipoMaterialesAsync(Guid id)
        {
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var multiple = await connection.QueryMultipleAsync(
                        @"SELECT l.Id, l.Descripcion
                    FROM     dbo.TipoMateriales l Where Id = @Id Order by l.Descripcion;"
                    , param: new { id });

                    var tipoMaterial = multiple.Read<tipoMaterialDTO>().ToList();

                    if (tipoMaterial.Count() == 0) throw new NotFoundException();

                    return tipoMaterial.First();
                }
            }
        }

        public async Task<IEnumerable<tipoMaterialDTO>> GetTipoMaterialesByNameAsync(string descripcion)
        {
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var multiple = await connection.QueryMultipleAsync(
                        @"SELECT l.Id, l.Descripcion
                    FROM     dbo.TipoMateriales l Where rtrim(ltrim(l.Descripcion)) like '%' + @descripcion + '%' Order by l.Descripcion;"
                    , param: new { descripcion });

                    var tipoMaterial = multiple.Read<tipoMaterialDTO>().ToList();

                    if (tipoMaterial.Count() == 0) throw new NotFoundException();

                    return tipoMaterial;
                }
            }
        }

        async Task<IEnumerable<tipoMaterialDTO>> ITipoMaterialesQueries.GetAll()
        {
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();

                    var multiple = await connection.QueryMultipleAsync(
                        @"SELECT l.Id, l.Descripcion
                    FROM     dbo.TipoMateriales l Order by l.Descripcion;");

                    var tipoMaterial = multiple.Read<tipoMaterialDTO>().ToList();

                    if (tipoMaterial.Count() == 0) throw new NotFoundException();

                    return tipoMaterial;
                }
            }
        }
    }

}