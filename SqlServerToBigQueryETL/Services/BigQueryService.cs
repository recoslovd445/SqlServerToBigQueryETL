using System.Text;
using System.Text.Json;
using Google.Cloud.BigQuery.V2;
using SqlServerToBigQueryETL.Models;
using SqlServerToBigQueryETL.Services.Interface;

namespace SqlServerToBigQueryETL.Services;

public class BigQueryService : IBigQueryService
{
    private readonly BigQueryClient _client;
    private readonly string _datasetId = "db_envios";
    private readonly string _tableId = "reporte_envios";

    public BigQueryService(BigQueryClient client)
    {
        _client = client;
    }

    public async Task<int> CargarDatosPorRangoAsync(IEnumerable<EnvioReporteModel> listaDatos, int diasAtras)
    {
        try
        {
            string fechaInicio = DateTime.Now.AddDays(-diasAtras).ToString("yyyy-MM-dd");

            string deleteSql = $"DELETE FROM `{_datasetId}.{_tableId}` WHERE `fecha_cosecha` >= '{fechaInicio}'";
            await _client.ExecuteQueryAsync(deleteSql, null);
            
            var filasParaSerializar = listaDatos.Select(d => new 
            {
                num_pallet = d.NumPallet,
                unidad_agricola = d.UnidadAgricola,
                materia_prima = d.MateriaPrima,
                tipo_cosecha = d.TipoCosecha,
                formato = d.Formato,
                num_jabas = (double)d.NumJabas,
                calibre = d.Calibre,
                tipo_pallet = d.TipoPallet,
                cantidad = d.Cantidad.HasValue ? (long)d.Cantidad.Value : 0,
                peso_planta = (long)(d.PesoPlanta ?? 0),
                peso_bruto = (double)(d.PesoBruto ?? 0),
                tipo_envase = d.TipoEnvase,
                tipo_envase_secundario = d.TipoEnvaseSecundario,
                peso_campo = (double)d.PesoCampo,
                fecha_cosecha = d.FechaCosecha.ToString("yyyy-MM-dd")
            }).ToList();

            if (filasParaSerializar.Any())
            {
                var jsonContent = string.Join("\n", filasParaSerializar.Select(f => JsonSerializer.Serialize(f)));
                using var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonContent));

                var options = new UploadJsonOptions { WriteDisposition = WriteDisposition.WriteAppend };
                var job = await _client.UploadJsonAsync(_datasetId, _tableId, null, stream, options);

                await job.PollUntilCompletedAsync();
                return filasParaSerializar.Count;
            }
            return 0;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error en BigQuery: {ex.Message}");
        }
    }
}

// Antes hicimos await _client.InsertRowsAsync(_datasetId, _tableId, filas); pero recordar que InsertRowsAsync es para un proceso de streaming