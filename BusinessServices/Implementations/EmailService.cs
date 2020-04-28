using AutoMapper;
using BusinessServices.Contracts;
using Models.BO;
using Models.ViewModels;
using Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BusinessServices.Implementations
{
    public class EmailService : IEmailService
    {
        IRepositoryWrapper _repositoryWrapper;

        public EmailService(IRepositoryWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }

        public void SendEmail(List<string> toList, List<string> ccList, string subject, string body)
        {
            MailMessage msg = new MailMessage();
            toList.ForEach(to => msg.To.Add(new MailAddress(to)));
            ccList.ForEach(cc => msg.CC.Add(new MailAddress(cc)));

            msg.From = new MailAddress("", "UCI-TMS");
            msg.Subject = subject;
            msg.Body = body;
            msg.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();
            client.UseDefaultCredentials = false;
            client.Credentials = new System.Net.NetworkCredential("", "");
            client.Port = 587; // You can use Port 25 if 587 is blocked (mine is!)
            client.Host = "smtp.office365.com";
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            try
            {
                client.Send(msg);               
            }
            catch (Exception ex)
            {
            }
        }    
    }
}
