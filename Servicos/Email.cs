using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace ProjetoAlugar.Servicos
{
    public class Email : IEmail
    {
        private ConfiguracaoEmail _configuracoesEmail;

        public Email(IOptions<ConfiguracaoEmail> configuracoesEmail)
        {
            _configuracoesEmail = configuracoesEmail.Value;
        }

        public async Task EnviarEmail(string email, string assunto, string mensagem)
        {
            var destinatario = String.IsNullOrEmpty(email) ? _configuracoesEmail.Email : email;

            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(_configuracoesEmail.Email, "NomePessoa")
            };

            mailMessage.To.Add(new MailAddress(destinatario));
            mailMessage.Subject = assunto;
            mailMessage.Body = mensagem;
            mailMessage.IsBodyHtml = true;
            mailMessage.Priority = MailPriority.High;

            using (SmtpClient smtpClient = new SmtpClient(_configuracoesEmail.Endereco, _configuracoesEmail.Porta))
            {
                smtpClient.Credentials = new NetworkCredential(_configuracoesEmail.Email, _configuracoesEmail.Senha);
                smtpClient.EnableSsl = true;
                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}
