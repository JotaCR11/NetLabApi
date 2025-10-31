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
        Task<(bool Exito, string MensajeError)> EnviarCorreoAsync(string asunto, string mensaje, string correoDestino);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<(bool Exito, string MensajeError)> EnviarCorreoAsync(string asunto, string mensaje, string correoDestino)
        {
            try
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
                    From = new MailAddress(correoRemitente,"Netlab",Encoding.UTF8),
                    Subject = asunto,
                    SubjectEncoding = Encoding.UTF8,
                    Body = mensaje,
                    BodyEncoding = Encoding.UTF8,
                    IsBodyHtml = true
                };
                foreach (var email in correoDestino.Split(','))
                {
                    mailMessage.HeadersEncoding = Encoding.UTF8;
                    mailMessage.Bcc.Add(email.Trim());
                }
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(mensaje, Encoding.UTF8, "text/html");
                mailMessage.AlternateViews.Add(htmlView);

                await client.SendMailAsync(mailMessage);
                return (true, string.Empty);
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }

        }

        public static async Task<string> CargarPlantillaAsync(string archivo, Dictionary<string, string> valores)
        {
            var rutaBase = AppContext.BaseDirectory;
            var rutaProyecto = Directory.GetParent(rutaBase)!.Parent!.Parent!.Parent!.Parent!.FullName;

            var ruta = Path.Combine(
                rutaProyecto,
                "Netlab.04.Infrastructure",
                "Templates"
            );
            var rutaArchivo = Path.Combine(ruta,archivo);

            string html = await File.ReadAllTextAsync(rutaArchivo, Encoding.UTF8);

            foreach (var kvp in valores)
                html = html.Replace($"{{{{{kvp.Key}}}}}", kvp.Value);

            return html;
        }
    }
}
