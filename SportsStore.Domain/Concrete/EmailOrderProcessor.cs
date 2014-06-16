using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;

namespace SportsStore.Domain.Concrete
{
    public class EmailOrderProcessor : IOrderProcessor
    {
        private EmailSettings emailSettings;

        public EmailOrderProcessor(EmailSettings settings)
        {
            emailSettings = settings;
        }

        public void ProcessorOrder(Cart cart, ShippingDetails shipping)
        {
            using(var smtpClient = new SmtpClient(emailSettings.Servername, emailSettings.Serverport))
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(emailSettings.Username, emailSettings.Password);

                if(emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation = emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder();
                body.AppendLine("A new order has been submitted");
                body.AppendLine("---");
                body.AppendLine("Items: ");

                foreach (var item in cart.Lines)
                {
                    var subtotal = item.Product.Price*item.Quantity;
                    body.AppendFormat("{0} x {1} (subtotal: {2})", item.Quantity, item.Product.Price, subtotal.ToString("c"));
                }

                body.AppendFormat("Total order value: {0}", cart.ComputeTotalValue().ToString("c"))
                    .AppendLine("---")
                    .AppendLine("Ship to: ")
                    .AppendLine(shipping.Name)
                    .AppendLine(shipping.Line1)
                    .AppendLine(shipping.Line2 ?? "")
                    .AppendLine(shipping.Line3 ?? "")
                    .AppendLine(shipping.City)
                    .AppendLine(shipping.State ?? "")
                    .AppendLine(shipping.Country)
                    .AppendLine(shipping.Zip)
                    .AppendLine("---")
                    .AppendFormat("Gift wrap: {0}",  shipping.GiftWrap ? "Yes" : "No")
                    ;

                MailMessage mailMessage = new MailMessage(emailSettings.MailFromAddress, emailSettings.MailToAddress, "New order submitted!", body.ToString());

                if(emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.ASCII;
                }

                smtpClient.Send(mailMessage);
            }
            
        }
    }

    public class EmailSettings
    {
        public string MailToAddress = "order@example.com";
        public string MailFromAddress = "sportsstore@example.com";
        public bool UseSsl = true;
        public string Username = "MySmtpUsername";
        public string Password= "password";
        public string Servername= "smtp.example.com";
        public int Serverport= 587;
        public bool WriteAsFile= true;
        public string FileLocation= @"c:\sports_store_emails";
    }
}
