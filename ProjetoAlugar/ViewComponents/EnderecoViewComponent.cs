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
    public class EnderecoViewComponent : ViewComponent
    {
        private readonly Contexto _context;
        private readonly IUsuario _usuario;

        public EnderecoViewComponent(Contexto context, IUsuario usuario)
        {
            _context = context;
            _usuario = usuario;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var usuario = await _usuario.BuscarUsuarioLogado(HttpContext.User);
            return View(await _context.Enderecos.Where(e => e.UsuarioId == usuario.Id).ToListAsync());
        }
    }
}
