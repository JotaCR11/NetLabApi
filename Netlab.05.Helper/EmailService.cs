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
        Task EnviarCorreoAsync(string asunto, string mensaje);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EnviarCorreoAsync(string asunto, string mensaje)
        {
            var smtp = _configuration["Smtp:Host"];
            var puerto = int.Parse(_configuration["Smtp:Port"]);
            var correoRemitente = _configuration["Smtp:From"];
            var clave = _configuration["Smtp:Password"];
            var correoDestino = _configuration["Smtp:To"];

            var client = new SmtpClient(smtp, puerto)
            {
                Credentials = new NetworkCredential(correoRemitente, clave),
                EnableSsl = true,
                UseDefaultCredentials = false
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(correoRemitente),
                Subject = asunto,
                Body = mensaje,
                IsBodyHtml = false
            };

            mailMessage.To.Add("josechavezrodriguez@gmail.com");
            foreach (var email in correoDestino.Split(','))
            {
                mailMessage.Bcc.Add(email.Trim());
            }

            await client.SendMailAsync(mailMessage);

        }
    }
}
