using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FreeIntegration.Services.EmailService
{
    public class EmailSender : BaseService,IEmailSender
    {
        public EmailSender(IConfiguration configuration) : base(configuration)
        {

        }
        public Task SendEmailAsync(string email,string subject,string body)
        {
            return Execute(Configuration.GetValue<string>("SendGridKey"),email,subject,body);
        }
        private Task Execute(string SendGridKey,string email,string subject,string body)
        {
            var client = new SendGridClient(SendGridKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("ozgurcatmakas@outlook.com","Free Integration"),
                Subject = subject,
                PlainTextContent = body,
                HtmlContent = body
            };
            msg.AddTo(new EmailAddress(email));
            try
            {
                var result = client.SendEmailAsync(msg);
                return result;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
    }
}
