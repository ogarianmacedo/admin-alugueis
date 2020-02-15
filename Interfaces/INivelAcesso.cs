using ProjetoAlugar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoAlugar.Interfaces
{
    public interface INivelAcesso : IRepositorioGenerico<NivelAcesso>
    {
        Task<bool> NivelAcessoExiste(string nivelAcesso);
    }
}
