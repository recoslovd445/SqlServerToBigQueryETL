using SqlServerToBigQueryETL.Models;

namespace SqlServerToBigQueryETL.Services.Interface;

public interface IBigQueryService
{
    Task<int> CargarDatosPorRangoAsync(IEnumerable<EnvioReporteModel> listaDatos, int diasAtras);
}