using SqlServerToBigQueryETL.Services.Interface;

namespace SqlServerToBigQueryETL.EndPoint;

public static class ExecuteEtlEndPoint
{
    public static void MapEtlEndPoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/ejecutar-etl", async (ISqlService sqlService, IBigQueryService bqService) =>
        {
            try
            {
                int days = 30;
                var datos = await sqlService.GetEnvioReporteModels(days);

                if (datos.Any())
                {
                    int procesados = await bqService.CargarDatosPorRangoAsync(datos, days);
                    return Results.Ok(new
                    {
                        estado = "Exitoso",
                        mensaje = $"Se actualizaron {procesados} registros en BigQuery.", 
                        fecha_ejecucion = DateTime.Now,
                    });
                }
                return Results.Ok(new { estado = "Sin Datos", mensaje = "No se encontraron registros en SQL Server." });
            }
            catch (Exception ex)
            {
                return Results.Problem(detail: ex.Message, title: "Error en el proceso ETL");
            }
        });
        
        app.MapGet("/", () => Results.Ok(new { app = "ETL SQL-to-BigQuery", status = "Online" }));
    }
}