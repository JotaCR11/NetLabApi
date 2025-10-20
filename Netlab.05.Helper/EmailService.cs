using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Netlab.Helper
{
    public interface IEmailService
    {
        Task EnviarCorreoAsync(string asunto, string mensaje, string correoDestino);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EnviarCorreoAsync(string asunto, string mensaje, string correoDestino)
        {
            var smtp = _configuration["Smtp:Host"];
            var puerto = int.Parse(_configuration["Smtp:Port"]);
            var correoRemitente = _configuration["Smtp:From"];
            var clave = _configuration["Smtp:Password"];
            //var correoDestino = _configuration["Smtp:To"];

            var client = new SmtpClient(smtp, puerto)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(correoRemitente, clave),
                EnableSsl = true // Para Gmail con puerto 587 o 465
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(correoRemitente),
                Subject = asunto,
                Body = mensaje,
                IsBodyHtml = false
            };
            foreach (var email in correoDestino.Split(','))
            {
                mailMessage.Bcc.Add(email.Trim());
            }

            await client.SendMailAsync(mailMessage);

        }
    }
}
