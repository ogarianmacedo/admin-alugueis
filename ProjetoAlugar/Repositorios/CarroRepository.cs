using ProjetoAlugar.Interfaces;
using ProjetoAlugar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoAlugar.Repositorios
{
    public class CarroRepository : RepositorioGenerico<Carro>, ICarro
    {
        public CarroRepository(Contexto contexto) : base(contexto)
        {
        }
    }
}
