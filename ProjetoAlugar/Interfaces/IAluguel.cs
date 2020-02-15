using ProjetoAlugar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoAlugar.Interfaces
{
    public interface IAluguel : IRepositorioGenerico<Aluguel>
    {
        Task<bool> VerificarReserva(string usuarioId, int carroId, string dataInicio, string dataFim);
    }
}
