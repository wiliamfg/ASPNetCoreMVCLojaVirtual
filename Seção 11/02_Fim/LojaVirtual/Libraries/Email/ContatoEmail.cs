using LojaVirtual.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace LojaVirtual.Libraries.Email
{
    public class ContatoEmail
    {
        public static void EnviarContatoPorEmail(Contato contato)
        {
            //SMTP -> Servidor que vai enviar a mensagem
            SmtpClient smtp = new SmtpClient("smtp.mail.yahoo.com", 587);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential("wiliamfg@yahoo.com.br", "senha");
            smtp.EnableSsl = true;

            string corpoMsg = string.Format("<h2>Contato - LojaVirtual</h2> <br />" +
                "<b>Nome: </b> {0} <br />" +
                "<b>E-mail: </b> {1} <br />" +
                "<b>Texto: </b> {2} <br />" +
                "<br />" +
                "E-mail enviado automaticamente do site LojaVirtual.",
                contato.Nome,
                contato.Email,
                contato.Texto
                );

            //MailMessage -> Construir a mensagem
            MailMessage mensagem = new MailMessage();
            mensagem.From = new MailAddress("wiliamfg@yahoo.com.br");
            mensagem.To.Add("wiliamfg@gmail.com");
            mensagem.Subject = "Contato - LojaVirtual - E-mail: " + contato.Email;
            mensagem.Body = corpoMsg;
            mensagem.IsBodyHtml = true;

            //Enviar mensagem
            smtp.Send(mensagem);
        }
    }
}
