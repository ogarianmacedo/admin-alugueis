using Microsoft.AspNetCore.Identity;
using ProjetoAlugar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjetoAlugar.Interfaces
{
    public interface IUsuario : IRepositorioGenerico<Usuario>
    {
        Task<Usuario> BuscarUsuarioLogado(ClaimsPrincipal usuario);
        Task<IdentityResult> SalvarUsuario(Usuario usuario, string senha);
        Task EditarUsuario(Usuario usuario);
        Task AtribuirNivelAcesso(Usuario usuario, string nivelAcesso);
        Task EfetuarLogin(Usuario usuario, bool lembrar);
        Task EfetuarLogout();
        Task<Usuario> BuscarUsuarioPorEmail(string email);
    }
}
