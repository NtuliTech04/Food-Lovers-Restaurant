using Microsoft.AspNet.Identity;
using System;
using System.Configuration;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using MailKit.Security;

namespace FLRApplication.Utilities
{
    public class SendEmail
    {
        public void Mail(IdentityMessage message)
        {
            #region formatter
            string text = string.Format(message.Subject, message.Body);
            string html = message.Body;
            #endregion

            MailMessage msg = new MailMessage();
            msg.From = new MailAddress(ConfigurationManager.AppSettings["Email"].ToString(), "Food Lover's Restaurant");
            msg.To.Add(new MailAddress(message.Destination));
            msg.Subject = message.Subject;
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
            msg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));


            SmtpClient smtpClient = new SmtpClient();

            NetworkCredential credentials = new NetworkCredential
            (ConfigurationManager.AppSettings["Email"].ToString(), 
            ConfigurationManager.AppSettings["Password"].ToString());

            smtpClient.Host = "smtp.gmail.com";
            smtpClient.Port = 587;
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = credentials;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.Send(msg);
        }
    }
}