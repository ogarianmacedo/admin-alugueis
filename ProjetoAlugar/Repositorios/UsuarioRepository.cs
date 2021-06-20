using Microsoft.AspNetCore.Identity;
using ProjetoAlugar.Interfaces;
using ProjetoAlugar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjetoAlugar.Repositorios
{
    public class UsuarioRepository : RepositorioGenerico<Usuario>, IUsuario
    {
        private readonly SignInManager<Usuario> _gerenciadorLogin;
        private readonly UserManager<Usuario> _gerenciadorUsuario;

        public UsuarioRepository(
            Contexto contexto,
            SignInManager<Usuario> gerenciadorLogin,
            UserManager<Usuario> gerenciadorUsuario
        ) : base (contexto)
        {
            _gerenciadorLogin = gerenciadorLogin;
            _gerenciadorUsuario = gerenciadorUsuario;
        }

        public async Task<Usuario> BuscarUsuarioLogado(ClaimsPrincipal usuario)
        {
            return await _gerenciadorUsuario.GetUserAsync(usuario);
        }

        public async Task<IdentityResult> SalvarUsuario(Usuario usuario, string senha)
        {
            return await _gerenciadorUsuario.CreateAsync(usuario, senha);
        }

        public async Task AtribuirNivelAcesso(Usuario usuario, string nivelAcesso)
        {
            await _gerenciadorUsuario.AddToRoleAsync(usuario, nivelAcesso);
        }

        public async Task EfetuarLogin(Usuario usuario, bool lembrar)
        {
            await _gerenciadorLogin.SignInAsync(usuario, lembrar);
        }

        public async Task EfetuarLogout()
        {
            await _gerenciadorLogin.SignOutAsync();
        }

        public async Task<Usuario> BuscarUsuarioPorEmail(string email)
        {
            return await _gerenciadorUsuario.FindByEmailAsync(email);
        }

        public async Task EditarUsuario(Usuario usuario)
        {
            await _gerenciadorUsuario.UpdateAsync(usuario);
        }
    }
}
