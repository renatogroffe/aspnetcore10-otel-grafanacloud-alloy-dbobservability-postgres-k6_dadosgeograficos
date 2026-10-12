namespace APIDadosGeograficosORM.Models;

public class DetalhesEstado
{
    public string? SiglaEstado { get; set; }
    public string? NomeEstado { get; set; }
    public string? NomeCapital { get; set; }
    public string? NomeRegiao { get; set; }
}

public class EstadoResumo
{
    public string? SiglaEstado { get; set; }
    public string? NomeEstado { get; set; }
    public string? NomeCapital { get; set; }
}

public class DetalhesRegiao
{
    public int IdRegiao { get; set; }
    public string? CodRegiao { get; set; }
    public string? NomeRegiao { get; set; }
    public List<EstadoResumo> Estados { get; set; } = new();
}
