using Microsoft.EntityFrameworkCore;
using ProjetoAlugar.Interfaces;
using ProjetoAlugar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoAlugar.Repositorios
{
    public class AluguelRepository : RepositorioGenerico<Aluguel>, IAluguel
    {
        private readonly Contexto _contexto;

        public AluguelRepository(Contexto contexto) : base (contexto)
        {
            _contexto = contexto;
        }

        public async Task<bool> VerificarReserva(string usuarioId, int carroId, string dataInicio, string dataFim)
        {
            return await _contexto.Alugueis.AnyAsync(
                    a => a.UsuarioId == usuarioId
                    && a.CarroId == carroId
                    && a.Inicio == dataInicio
                    && a.Fim == dataFim
                );
        }
    }
}
