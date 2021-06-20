using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoAlugar.ViewsModels
{
    public class AluguelViewModel
    {
        public int CarroId { get; set; }

        public int PrecoDiaria { get; set; }

        public string Inicio { get; set; }

        public string Fim { get; set; }

        public int PrecoTotal { get; set; }
    }
}
