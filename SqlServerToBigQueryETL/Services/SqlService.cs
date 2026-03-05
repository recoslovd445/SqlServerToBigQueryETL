using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using SqlServerToBigQueryETL.Models;
using SqlServerToBigQueryETL.Services.Interface;

namespace SqlServerToBigQueryETL.Services;

public class SqlService : ISqlService
{
    private readonly string _connectionString;
    
    public SqlService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    }

    public async Task<List<EnvioReporteModel>> GetEnvioReporteModels(int diasAtras = 3)
    {
        try
        {
            using var conection = new SqlConnection(_connectionString);

            var parameters = new { DiasAtras = diasAtras };

            Console.WriteLine($"[SQL] Extrayendo datos de los últimos {diasAtras} días...");

            var resultado = await conection.QueryAsync<EnvioReporteModel>(
                "envio.sp_GetEnviosParaBigQuery",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var lista = resultado.ToList();
            Console.WriteLine($"[SQL] Se extrajeron {lista.Count} registros con éxito.");

            return lista;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error SQL] Ocurrió un problema al extraer: {ex.Message}");
            return new List<EnvioReporteModel>();
        }
    }
}