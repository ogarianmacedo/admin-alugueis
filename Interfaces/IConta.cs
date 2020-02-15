using ProjetoAlugar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoAlugar.Interfaces
{
    public interface IConta : IRepositorioGenerico<Conta>
    {
        new Task<IEnumerable<Conta>> BuscarTodas();
        int BuscarSaldoPeloId(string Id);
    }
}
