﻿using SendGrid;
using SendGrid.Helpers.Mail;
using tickets.Models;

namespace tickets.Servicios
{
    public interface IServicioEmail
    {
        Task EnviarConfirmacionTicket(Guid id, string correo);
    }
    public class ServicioEmail : IServicioEmail
    {
        private readonly IConfiguration _configuration;

        public ServicioEmail(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EnviarConfirmacionTicket(Guid id, string correo)
        {
            var apiKey = _configuration.GetValue<string>("SENDGRID_API_KEY");
            var email = _configuration.GetValue<string>("SENGRID_FROM");
            var nombre = _configuration.GetValue<string>("SENDGRID_NOMBRE");

            var cliete = new SendGridClient(apiKey);
            var from =  new EmailAddress(email, nombre);
            var subje = $"Su Ticket ha sido Creado con Éxito";
            var to = new EmailAddress(correo, nombre);

            string textoPlano = "Su ticket ha sido creado éxitosamente, al surgir un cambio en el proceso del mismo, se le notificará por este mismo medio.";
            var contenidoHtml = $@"Su ticket ha sido creado éxitosamente, al surgir un cambio en el proceso del mismo, se le notificará por este mismo medio.
                                   su número de ticket es: {id}
                                   De: Ticket Master";

            var singleMail = MailHelper.CreateSingleEmail(from, to, subje, textoPlano, contenidoHtml);

            var respuesta = await cliete.SendEmailAsync(singleMail);

        }
    }
}
