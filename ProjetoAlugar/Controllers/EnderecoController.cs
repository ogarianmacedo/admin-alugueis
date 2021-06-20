using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjetoAlugar.Interfaces;
using ProjetoAlugar.Models;

namespace ProjetoAlugar.Controllers
{
    [Authorize]
    public class EnderecoController : Controller
    {
        private readonly IUsuario _usuarioRepositorio;
        private readonly IEndereco _enderecoRepositorio;

        public EnderecoController(
            IUsuario usuarioRepositorio,
            IEndereco enderecoRepositorio
        )
        {
            _usuarioRepositorio = usuarioRepositorio;
            _enderecoRepositorio = enderecoRepositorio;
        }

        public async Task<IActionResult> Create()
        {
            var usuario = await _usuarioRepositorio.BuscarUsuarioLogado(User);
            var endereco = new Endereco { UsuarioId = usuario.Id };
            return View(endereco);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EnderecoId,Rua,Numero,Bairro,Cidade,Estado,UsuarioId")] Endereco endereco)
        {
            if (ModelState.IsValid)
            {
                await _enderecoRepositorio.Inserir(endereco);
                return RedirectToAction("Index", "Usuario");
            }

            return View(endereco);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var endereco = await _enderecoRepositorio.BuscarPeloId(id);
            if (endereco == null)
            {
                return NotFound();
            }

            return View(endereco);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EnderecoId,Rua,Numero,Bairro,Cidade,Estado,UsuarioId")] Endereco endereco)
        {
            if (id != endereco.EnderecoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _enderecoRepositorio.Atualizar(endereco);
                return RedirectToAction("Index", "Usuario");
            }

            return View(endereco);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            await _enderecoRepositorio.Excluir(id);
            return Json("Endereço excluído");
        }
    }
}
