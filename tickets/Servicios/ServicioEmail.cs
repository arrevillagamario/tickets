using SendGrid;
using SendGrid.Helpers.Mail;
using tickets.Models;

namespace tickets.Servicios
{
    public interface IServicioEmail
    {
        Task ContactarTicket(Guid id, string correo, string contenidoHtml);
        Task EnviarAsignacionTicket(Guid id, string correo);
        Task EnviarConfirmacionTicket(Guid id, string correo);
        Task EnviarFinalizaciónTicket(Guid id, string correo);
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
                                   su número de ticket es: {id }     
                                     De: Ticket Master";

            var singleMail = MailHelper.CreateSingleEmail(from, to, subje, textoPlano, contenidoHtml);

            var respuesta = await cliete.SendEmailAsync(singleMail);

        }


        public async Task EnviarFinalizaciónTicket(Guid id, string correo)
        {
            var apiKey = _configuration.GetValue<string>("SENDGRID_API_KEY");
            var email = _configuration.GetValue<string>("SENGRID_FROM");
            var nombre = _configuration.GetValue<string>("SENDGRID_NOMBRE");

            var cliete = new SendGridClient(apiKey);
            var from = new EmailAddress(email, nombre);
            var subje = $"Su Ticket ha sido resuelto con Éxito";
            var to = new EmailAddress(correo, nombre);

            string textoPlano = "Su ticket ha sido resuelto éxitosamente";
            var contenidoHtml = $@"Su ticket: {id} ha sido resuelto con éxito. 
                            
                                     De: Ticket Master";

            var singleMail = MailHelper.CreateSingleEmail(from, to, subje, textoPlano, contenidoHtml);

            var respuesta = await cliete.SendEmailAsync(singleMail);

        }

        public async Task EnviarAsignacionTicket(Guid id, string correo)
        {
            var apiKey = _configuration.GetValue<string>("SENDGRID_API_KEY");
            var email = _configuration.GetValue<string>("SENGRID_FROM");
            var nombre = _configuration.GetValue<string>("SENDGRID_NOMBRE");

            var cliete = new SendGridClient(apiKey);
            var from = new EmailAddress(email, nombre);
            var subje = $"Nuevo ticket asignado";
            var to = new EmailAddress(correo, nombre);

            string textoPlano = "Un nuevo Ticket se le ha sido asignado";
            var contenidoHtml = $@"El ticket con id: {id} se le ha sido asignado para su resolución.\n De: Ticket Master";

            var singleMail = MailHelper.CreateSingleEmail(from, to, subje, textoPlano, contenidoHtml);

            var respuesta = await cliete.SendEmailAsync(singleMail);

        }

        public async Task ContactarTicket(Guid id, string correo, string contenidoHtml )
        {
            var apiKey = _configuration.GetValue<string>("SENDGRID_API_KEY");
            var email = _configuration.GetValue<string>("SENGRID_FROM");
            var nombre = _configuration.GetValue<string>("SENDGRID_NOMBRE");

            var cliete = new SendGridClient(apiKey);
            var from = new EmailAddress(email, nombre);
            var subje = $"Se Solicita Información de su caso.";
            var to = new EmailAddress(correo, nombre);

            string textoPlano = $"Se necesita información respecto a su ticket: {id}";
        
            var singleMail = MailHelper.CreateSingleEmail(from, to, subje, textoPlano, contenidoHtml);

            var respuesta = await cliete.SendEmailAsync(singleMail);

        }
    }
}
