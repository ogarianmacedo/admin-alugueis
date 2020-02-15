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
    public class NivelAcessoController : Controller
    {
        private readonly INivelAcesso _nivelAcessoRepositorio;

        public NivelAcessoController(INivelAcesso nivelAcessoRepositorio)
        {
            _nivelAcessoRepositorio = nivelAcessoRepositorio;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _nivelAcessoRepositorio.BuscarTodos().ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Descricao,Name")] NivelAcesso nivelAcesso)
        {
            if (ModelState.IsValid)
            { 
                bool nivelExiste = await _nivelAcessoRepositorio.NivelAcessoExiste(nivelAcesso.Name);

                if (!nivelExiste)
                {
                    nivelAcesso.NormalizedName = nivelAcesso.Name.ToUpper();
                    await _nivelAcessoRepositorio.Inserir(nivelAcesso);
                    return RedirectToAction("Index", "NivelAcesso");
                }
            }

            return View(nivelAcesso);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nivelAcesso = await _nivelAcessoRepositorio.BuscarPeloId(id);
            if (nivelAcesso == null)
            {
                return NotFound();
            }
            return View(nivelAcesso);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Descricao,Id,Name,NormalizedName,ConcurrencyStamp")] NivelAcesso nivelAcesso)
        {
            if (id != nivelAcesso.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                nivelAcesso.NormalizedName = nivelAcesso.Name.ToUpper();
                await _nivelAcessoRepositorio.Atualizar(nivelAcesso);
                return RedirectToAction("Index", "NivelAcesso");
            }

            return View(nivelAcesso);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await _nivelAcessoRepositorio.Excluir(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
