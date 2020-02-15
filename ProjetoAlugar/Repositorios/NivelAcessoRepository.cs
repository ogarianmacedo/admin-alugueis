using Microsoft.AspNetCore.Identity;
using ProjetoAlugar.Interfaces;
using ProjetoAlugar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoAlugar.Repositorios
{
    public class NivelAcessoRepository : RepositorioGenerico<NivelAcesso>, INivelAcesso
    {
        private readonly RoleManager<NivelAcesso> _gerenciadorNivelAceso;
        private readonly Contexto _contexto;

        public NivelAcessoRepository(Contexto contexto, RoleManager<NivelAcesso> gerenciadorNivelAceso) : base(contexto)
        {
            _contexto = contexto;
            _gerenciadorNivelAceso = gerenciadorNivelAceso;
        }

        public async Task<bool> NivelAcessoExiste(string nivelAcesso)
        {
            return await _gerenciadorNivelAceso.RoleExistsAsync(nivelAcesso);
        }
    }
}
