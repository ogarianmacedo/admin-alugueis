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
    public class ContaController : Controller
    {
        private readonly IConta _contaRepositorio;
        private readonly IUsuario _usuarioRepositorio;

        public ContaController(
            IConta contaRepositorio,
            IUsuario usuarioRepositorio
        )
        {
            _contaRepositorio = contaRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _contaRepositorio.BuscarTodas());
        }

        public IActionResult Create()
        {
            ViewData["UsuarioId"] = new SelectList(_usuarioRepositorio.BuscarTodos(), "Id", "Email");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContaId,UsuarioId,Saldo")] Conta conta)
        {
            if (ModelState.IsValid)
            {
                await _contaRepositorio.Inserir(conta);
                return RedirectToAction(nameof(Index));
            }

            ViewData["UsuarioId"] = new SelectList(_usuarioRepositorio.BuscarTodos(), "Id", "Email", conta.UsuarioId);
            return View(conta);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var conta = await _contaRepositorio.BuscarPeloId(id);
            if (conta == null)
            {
                return NotFound();
            }

            ViewData["UsuarioId"] = new SelectList(_usuarioRepositorio.BuscarTodos(), "Id", "Email", conta.UsuarioId);
            return View(conta);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ContaId,UsuarioId,Saldo")] Conta conta)
        {
            if (id != conta.ContaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _contaRepositorio.Atualizar(conta);

                return RedirectToAction(nameof(Index));
            }

            ViewData["UsuarioId"] = new SelectList(await _contaRepositorio.BuscarTodas(), "Id", "Email", conta.UsuarioId);
            return View(conta);
        }
       
    }
}
