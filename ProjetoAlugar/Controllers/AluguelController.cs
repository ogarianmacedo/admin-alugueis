using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoAlugar.Interfaces;
using ProjetoAlugar.Models;
using ProjetoAlugar.Servicos;
using ProjetoAlugar.ViewsModels;

namespace ProjetoAlugar.Controllers
{
    [Authorize]
    public class AluguelController : Controller
    {
        private readonly IUsuario _usuarioRepositorio;
        private readonly IConta _contaRepositorio;
        private readonly IAluguel _aluguelRepositorio;
        private readonly IEmail _email;

        public AluguelController(
            IUsuario usuarioRepositorio,
            IConta contaRepositorio,
            IAluguel aluguelRepositorio,
            IEmail email
        )
        {
            _usuarioRepositorio = usuarioRepositorio;
            _contaRepositorio = contaRepositorio;
            _aluguelRepositorio = aluguelRepositorio;
            _email = email;
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

                    //Enviar E-mail
                    string assunto = "Reserva concluída com sucesso";
                    string mensagem = string.Format("Seu veículo já o aguarda. Você poderá pegá-lo dia {0} e deverá devolvê-lo dia {1}. O preço será R${2},00. Divirtá-se !!! ", aluguel.Inicio, aluguel.Fim, aluguel.PrecoTotal);
                    //await _email.EnviarEmail(usuario.Email, assunto, mensagem);

                    //Insere aluguel no banco
                    await _aluguelRepositorio.Inserir(a);

                    //Altera saldo do usuário
                    var saldoUsuario = await _contaRepositorio.BuscarSaldoPeloUsuarioId(usuario.Id);
                    saldoUsuario.Saldo = saldoUsuario.Saldo - aluguel.PrecoTotal;
                    await _contaRepositorio.Atualizar(saldoUsuario);

                    return RedirectToAction("Index", "Carro");
                }
            }

            return View("Aluguel", aluguel);
        }
    }
}