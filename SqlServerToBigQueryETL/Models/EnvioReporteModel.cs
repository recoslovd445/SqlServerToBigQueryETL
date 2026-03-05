namespace SqlServerToBigQueryETL.Models;

public class EnvioReporteModel
{
    public string? NumPallet { get; set; }
    
    public string? UnidadAgricola { get; set; }
    public string? MateriaPrima { get; set; }
    public string? TipoCosecha { get; set; }
    public string? Formato { get; set; }
    
    public decimal NumJabas { get; set; }
    
    public string? Calibre { get; set; }
    public string? TipoPallet { get; set; }
    
    public decimal? Cantidad { get; set; }
    public decimal? PesoPlanta { get; set; }
    public decimal? PesoBruto { get; set; }
    
    public string? TipoEnvase { get; set; }
    public string? TipoEnvaseSecundario { get; set; }
    
    public decimal PesoCampo { get; set; }
    public DateTime FechaCosecha { get; set; }
}