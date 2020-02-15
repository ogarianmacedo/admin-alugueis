using Microsoft.EntityFrameworkCore;
using ProjetoAlugar.Interfaces;
using ProjetoAlugar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoAlugar.Repositorios
{
    public class ContaRepository : RepositorioGenerico<Conta>, IConta
    {
        private readonly Contexto _contexto;

        public ContaRepository(Contexto contexto) : base(contexto)
        {
            _contexto = contexto;
        }

        public int BuscarSaldoPeloId(string Id)
        {
            return _contexto.Contas.FirstOrDefault(c => c.UsuarioId == Id).Saldo;
        }

        public new async Task<IEnumerable<Conta>> BuscarTodas()
        {
            return await _contexto.Contas.Include(c => c.Usuario).ToListAsync();
        }

        public async Task<Conta> BuscarSaldoPeloUsuarioId(string id)
        {
            return await _contexto.Contas.FirstOrDefaultAsync(c => c.UsuarioId == id);
        }
    }
}
