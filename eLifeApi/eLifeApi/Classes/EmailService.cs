using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

namespace eLifeApi.Classes
{
    public class EmailService : Controller
    {
        public string callbackUrl;

        private async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("Администрация сайта", "elifeprojectnure@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 465);
                await client.AuthenticateAsync("elifeprojectnure@gmail.com", "eLifeProject");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
        
        public async Task SendConfirmation(int userId, string code, string email, string callbackUrl)
        {
            await SendEmailAsync(
                email, 
                "Confirm your account",
                $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>"
                );
        }
    }
}