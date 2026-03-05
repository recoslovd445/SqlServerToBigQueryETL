using SqlServerToBigQueryETL.Models;

namespace SqlServerToBigQueryETL.Services.Interface;

public interface ISqlService
{
    Task<List<EnvioReporteModel>> GetEnvioReporteModels(int diasAtras = 3);
}