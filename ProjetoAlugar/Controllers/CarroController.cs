using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProjetoAlugar.Interfaces;
using ProjetoAlugar.Models;

namespace ProjetoAlugar.Controllers
{
    [Authorize]
    public class CarroController : Controller
    {
        private readonly ICarro _carroRepositorio;
        private readonly IHostingEnvironment _hostingEnvironment;

        public CarroController(
            ICarro carroRepositorio,
            IHostingEnvironment hostingEnvironment
        )
        {
            _carroRepositorio = carroRepositorio;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _carroRepositorio.BuscarTodos().ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CarroId,Nome,Marca,Foto,PrecoDiaria")] Carro carro, IFormFile arquivo)
        {
            if (ModelState.IsValid)
            {
                if(arquivo != null)
                {
                    var linkUpload = Path.Combine(_hostingEnvironment.WebRootPath, "Imagens");

                    using(FileStream fileStream = new FileStream(Path.Combine(linkUpload, arquivo.FileName), FileMode.Create))
                    {
                        await arquivo.CopyToAsync(fileStream);
                        carro.Foto = "~/Imagens/" + arquivo.FileName;
                    }
                }

                await _carroRepositorio.Inserir(carro);
                return RedirectToAction(nameof(Index));
            }

            return View(carro);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var carro = await _carroRepositorio.BuscarPeloId(id);
            if (carro == null)
            {
                return NotFound();
            }

            TempData["FotoCarro"] = carro.Foto;
            return View(carro);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CarroId,Nome,Marca,Foto,PrecoDiaria")] Carro carro, IFormFile arquivo)
        {
            if (id != carro.CarroId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (arquivo != null)
                {
                    var linkUpload = Path.Combine(_hostingEnvironment.WebRootPath, "Imagens");

                    using (FileStream fileStream = new FileStream(Path.Combine(linkUpload, arquivo.FileName), FileMode.Create))
                    {
                        await arquivo.CopyToAsync(fileStream);
                        carro.Foto = "~/Imagens/" + arquivo.FileName;
                    }
                }
                else
                {
                    carro.Foto = TempData["FotoCarro"].ToString();
                }

                await _carroRepositorio.Atualizar(carro);
                return RedirectToAction(nameof(Index));
            }
            return View(carro);
        }

        [HttpPost]
        public async Task<JsonResult> Delete(int id)
        {
            var carro = await _carroRepositorio.BuscarPeloId(id);
            string fotoCarro = carro.Foto;

            fotoCarro = fotoCarro.Replace("~", "wwwroot");
            System.IO.File.Delete(fotoCarro);

            await _carroRepositorio.Excluir(id);
            return Json("Carro excluído com sucesso");
        }

    }
}
