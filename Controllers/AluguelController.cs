using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProjetoAlugar.Interfaces;
using ProjetoAlugar.Models;
using ProjetoAlugar.ViewsModels;

namespace ProjetoAlugar.Controllers
{
    public class AluguelController : Controller
    {
        private readonly IUsuario _usuarioRepositorio;
        private readonly IConta _contaRepositorio;
        private readonly IAluguel _aluguelRepositorio;

        public AluguelController(
            IUsuario usuarioRepositorio, 
            IConta contaRepositorio, 
            IAluguel aluguelRepositorio
        )
        {
            _usuarioRepositorio = usuarioRepositorio;
            _contaRepositorio = contaRepositorio;
            _aluguelRepositorio = aluguelRepositorio;
        }

        public IActionResult Aluguel(int carroId, int precoDiaria)
        {
            var aluguel = new AluguelViewModel
            {
                CarroId = carroId,
                PrecoDiaria = precoDiaria
            };

            return View(aluguel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Alugar(AluguelViewModel aluguel)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _usuarioRepositorio.BuscarUsuarioLogado(User);
                var saldo = _contaRepositorio.BuscarSaldoPeloId(usuario.Id);

                if(await _aluguelRepositorio.VerificarReserva(usuario.Id, aluguel.CarroId, aluguel.Inicio, aluguel.Fim))
                {
                    TempData["Aviso"] = "Você já possui esse veículo";
                }
                else if (aluguel.PrecoTotal > saldo)
                {
                    TempData["Aviso"] = "Saldo insuficiente";
                }
                else
                {
                    Aluguel a = new Aluguel
                    {
                        UsuarioId = usuario.Id,
                        CarroId = aluguel.CarroId,
                        Inicio = aluguel.Inicio,
                        Fim = aluguel.Fim,
                        PrecoTotal = aluguel.PrecoTotal
                    };

                    await _aluguelRepositorio.Inserir(a);

                    var saldoUsuario = await _contaRepositorio.BuscarPeloId(usuario.Id);
                    saldoUsuario.Saldo = saldoUsuario.Saldo - aluguel.PrecoTotal;
                    await _contaRepositorio.Atualizar(saldoUsuario);

                    return RedirectToAction("Index", "Carro");
                }
            }

            return View(aluguel);
        }
    }
}