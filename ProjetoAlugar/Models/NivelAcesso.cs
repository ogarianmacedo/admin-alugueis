using Microsoft.AspNetCore.Identity;

namespace ProjetoAlugar.Models
{
    public class NivelAcesso : IdentityRole
    {
        public string Descricao { get; set; }
    }
}
