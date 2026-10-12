using APIDadosGeograficosORM.Models;
using Microsoft.EntityFrameworkCore;

namespace APIDadosGeograficosORM.Data;


public class DadosGeograficosRepository
{
    private readonly DadosGeograficosContext _context;

    public DadosGeograficosRepository(DadosGeograficosContext context)
    {
        _context = context;
    }

    public async Task<List<DetalhesEstado>> GetEstados()
    {
        return await _context.Estados
            .AsNoTracking()
            .Include(e => e.Regiao)
            .OrderBy(e => e.NomeEstado)
            .Select(e => new DetalhesEstado
            {
                SiglaEstado = e.SiglaEstado,
                NomeEstado = e.NomeEstado,
                NomeCapital = e.NomeCapital,
                NomeRegiao = e.Regiao != null ? e.Regiao.NomeRegiao : null
            })
            .ToListAsync();
    }

    public async Task<DetalhesEstado?> GetInfoEstado(string siglaEstado)
    {
        return await _context.Estados
            .AsNoTracking()
            .Include(e => e.Regiao)
            .Where(e => e.SiglaEstado == siglaEstado)
            .Select(e => new DetalhesEstado
            {
                SiglaEstado = e.SiglaEstado,
                NomeEstado = e.NomeEstado,
                NomeCapital = e.NomeCapital,
                NomeRegiao = e.Regiao != null ? e.Regiao.NomeRegiao : null
            })
            .FirstOrDefaultAsync();
    }

    public async Task<DetalhesRegiao?> GetDetalhesRegiao(string codRegiao)
    {
        return await _context.Regioes
            .AsNoTracking()
            .Include(r => r.Estados)
            .Where(r => r.CodRegiao == codRegiao)
            .Select(r => new DetalhesRegiao
            {
                IdRegiao = r.IdRegiao,
                CodRegiao = r.CodRegiao,
                NomeRegiao = r.NomeRegiao,
                Estados = r.Estados != null
                    ? r.Estados
                        .OrderBy(e => e.NomeEstado)
                        .Select(e => new EstadoResumo
                        {
                            SiglaEstado = e.SiglaEstado,
                            NomeEstado = e.NomeEstado,
                            NomeCapital = e.NomeCapital
                        })
                        .ToList()
                    : new List<EstadoResumo>()
            })
            .FirstOrDefaultAsync();
    }
}
