namespace OSPeConTI.BackEndBase.Services.CatalogoMateriales.Application.Queries
{
    using Dapper;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using System.Linq;

    public class ClasificacionesQueries
        : IClasificacionesQueries
    {
        private string _connectionString = string.Empty;

        public ClasificacionesQueries(string constr)
        {
            _connectionString = !string.IsNullOrWhiteSpace(constr) ? constr : throw new ArgumentNullException(nameof(constr));
        }

        public async Task<ClasificacionDTO> GetClasificacionesAsync(Guid id)

        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var multiple = await connection.QueryMultipleAsync(
                   @"SELECT c.Id, c.Descripcion
                    FROM     dbo.Clasificaciones c where c.Id = @id;"
                    , new { id });

                var clasificaciones = multiple.Read<ClasificacionDTO>().First();

                if (clasificaciones == null)
                    throw new KeyNotFoundException();

                return clasificaciones;
            }
        }

        public async Task<IEnumerable<ClasificacionDTO>> GetClasificacionesByNameAsync(string descripcion)

        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var multiple = await connection.QueryMultipleAsync(
                   @"SELECT c.Id, c.Descripcion
                    FROM     dbo.Clasificaciones c where c.Descripcion like '%' + @descripcion + '%' Order by c.Descripcion;"
                    , new { descripcion });

                var clasificaciones = multiple.Read<ClasificacionDTO>().ToList();

                if (clasificaciones == null)
                    throw new KeyNotFoundException();

                return clasificaciones;
            }
        }

        public async Task<IEnumerable<ClasificacionDTO>> GetAll()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                var multiple = await connection.QueryMultipleAsync(
                    @"SELECT c.Id, c.Descripcion
                    FROM     dbo.Clasificaciones c Order by c.Descripcion desc;");

                var clasificaciones = multiple.Read<ClasificacionDTO>().ToList();

                return clasificaciones;
            }
        }


    }

}