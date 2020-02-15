using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoAlugar.Interfaces;
using ProjetoAlugar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjetoAlugar.ViewComponents
{
    public class CarrosAlugadosViewComponent : ViewComponent
    {
        private readonly IUsuario _usuarioRepositorio;
        private readonly Contexto _contexto;

        public CarrosAlugadosViewComponent(IUsuario usuarioRepositorio, Contexto contexto)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _contexto = contexto;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var usuarioLogado = await _usuarioRepositorio.BuscarUsuarioLogado(HttpContext.User);

            return View(await _contexto.Alugueis.Include(a => a.Carro).Where(a => a.UsuarioId == usuarioLogado.Id).ToListAsync());
        }
    }
}
