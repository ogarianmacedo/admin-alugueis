using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProjetoAlugar.Interfaces;
using ProjetoAlugar.Models;
using ProjetoAlugar.ViewsModels;

namespace ProjetoAlugar.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly IUsuario _usuarioRepositorio;

        public UsuarioController(IUsuario usuarioRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _usuarioRepositorio.BuscarUsuarioLogado(User));
        }

        [AllowAnonymous]
        public async Task<IActionResult> Cadastro()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _usuarioRepositorio.EfetuarLogout();
            }

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cadastrar(RegistroViewModel registro)
        {
            if (ModelState.IsValid)
            {
                var usuario = new Usuario
                {
                    UserName = registro.NomeUsuario,
                    Email = registro.Email,
                    CPF = registro.CPF,
                    Telefone = registro.Telefone,
                    Nome = registro.Nome,
                    PasswordHash = registro.Senha
                };

                var resultado = await _usuarioRepositorio.SalvarUsuario(usuario, registro.Senha);

                if (resultado.Succeeded)
                {
                    var nivelAcesso = "Administrador";
                    await _usuarioRepositorio.AtribuirNivelAcesso(usuario, nivelAcesso);
                    await _usuarioRepositorio.EfetuarLogin(usuario, false);
                    return RedirectToAction("Index", "Usuario");
                }
                else
                {
                    foreach(var erro in resultado.Errors)
                    {
                        ModelState.AddModelError("", erro.Description.ToString());
                    }
                }
            }

            return View("Cadastro", registro);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _usuarioRepositorio.EfetuarLogout();
            }

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _usuarioRepositorio.BuscarUsuarioPorEmail(login.Email);
                PasswordHasher<Usuario> passwordHasher = new PasswordHasher<Usuario>();

                if(usuario != null)
                {
                    if(passwordHasher.VerifyHashedPassword(usuario, usuario.PasswordHash, login.Senha) != PasswordVerificationResult.Failed)
                    {
                        await _usuarioRepositorio.EfetuarLogin(usuario, false);
                        return RedirectToAction("Index", "Usuario");
                    }

                    ModelState.AddModelError("", "E-mail e/ou Senha inválidos");
                }

                ModelState.AddModelError("", "E-mail e/ou Senha inválidos");
            }

            return View(login);
        }

        public async Task<IActionResult> Sair()
        {
            await _usuarioRepositorio.EfetuarLogout();
            return RedirectToAction("Login", "Usuario");
        }

        [HttpGet]
        public async Task<IActionResult> Atualiza(string UsuarioId)
        {
            var usuario = await _usuarioRepositorio.BuscarPeloId(UsuarioId);

            var atualizaViewModel = new AtualizarUsuarioViewModel
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                CPF = usuario.CPF,
                Email = usuario.Email,
                Telefone = usuario.Telefone,
                NomeUsuario = usuario.UserName
            };

            return View(atualizaViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Atualizar(AtualizarUsuarioViewModel atualizarUsuario)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _usuarioRepositorio.BuscarPeloId(atualizarUsuario.Id);

                usuario.Nome = atualizarUsuario.Nome;
                usuario.CPF = atualizarUsuario.CPF;
                usuario.UserName = atualizarUsuario.NomeUsuario;
                usuario.Email = atualizarUsuario.Email;
                usuario.Telefone = atualizarUsuario.Telefone;

                await _usuarioRepositorio.EditarUsuario(usuario);
                return RedirectToAction("Index", "Usuario");
            }

            return View(atualizarUsuario);
        }
    }
}