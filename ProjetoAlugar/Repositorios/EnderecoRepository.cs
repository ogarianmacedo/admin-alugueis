using ProjetoAlugar.Interfaces;
using ProjetoAlugar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoAlugar.Repositorios
{
    public class EnderecoRepository : RepositorioGenerico<Endereco>, IEndereco
    {
        public EnderecoRepository(Contexto contexto) : base(contexto)
        {
        }
    }
}
