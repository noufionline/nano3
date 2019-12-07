using Jasmine.Core.Common;
using Jasmine.Core.Contracts;
using Microsoft.Office.Interop.Outlook;
using System;
using System.IO;
using System.Net.Mail;
using System.Text;
using Attachment = System.Net.Mail.Attachment;
using Exception = System.Exception;

namespace Jasmine.Core.Services
{
    public class OutLookEmailService : IEmailService
    {

        public bool SendMail(string fromAddress, string toAddress, string subject, string htmlBody, StreamAttachment attachment)
        {
            using (MailMessage mailMsg = new MailMessage())
            {

                try
                {
                    MailAddress mailAddress = new MailAddress(fromAddress);
                    // To
                    foreach (string x in toAddress.Split(';'))
                        mailMsg.To.Add(x);

                    mailMsg.From = mailAddress;
                    mailMsg.Subject = subject;
                    mailMsg.IsBodyHtml = true;
                    mailMsg.Body = subject;

                    if (attachment != null)
                        mailMsg.Attachments.Add(new Attachment(attachment.File, attachment.FileName));

                    //var credentials = new NetworkCredential();
                    //credentials.Domain = "192.168.30.200";
                    //credentials.UserName = "";
                    //var smtpClient = new SmtpClient("mail.emirates.net.ae", Convert.ToInt32(25)) { Credentials = new NetworkCredential() };
                    SmtpClient smtpClient = new SmtpClient("mail.emirates.net.ae");
                    //SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", Convert.ToInt32(25)) { Credentials = new NetworkCredential() };

                    smtpClient.Send(mailMsg);
                    mailMsg.Attachments.Clear();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }

            }
        }


        public bool SendMail(string fromAddress, string toEmailAddress, string subject, string htmlBody)
        {
            // Create the Outlook application.
            Application oApp = new Application();
            // Create a new mail item.
            MailItem oMsg = (MailItem)oApp.CreateItem(OlItemType.olMailItem);
            oMsg.BodyFormat = OlBodyFormat.olFormatHTML;
            // Set HTMLBody. 
            //add the body of the email

            StringBuilder msgBuilder = new StringBuilder(htmlBody);
            msgBuilder.AppendLine("<br>");
            msgBuilder.AppendLine(ReadSignature());

            oMsg.HTMLBody = msgBuilder.ToString();

            //Subject line
            oMsg.Subject = subject;
            oMsg.ReadReceiptRequested = true;
            oMsg.OriginatorDeliveryReportRequested = true;

            // Add a recipient.
            Recipients oRecips = oMsg.Recipients;
            // Change the recipient in the next line if necessary.
            foreach (string address in toEmailAddress.Split(';'))
            {
                if (string.IsNullOrWhiteSpace(address)) continue;
                oRecips.Add(address);
            }

            oRecips.ResolveAll();
            oMsg.Display(true);

            return true;
        }


        public bool SendMail(string fromAddress, string toEmailAddress, string subject, string htmlBody, string fileName, string path, bool isModel)
        {
            // Create the Outlook application.
            Application oApp = new Application();
            // Create a new mail item.
            MailItem oMsg = (MailItem)oApp.CreateItem(OlItemType.olMailItem);
            oMsg.BodyFormat = OlBodyFormat.olFormatHTML;
            // Set HTMLBody. 
            //add the body of the email

            StringBuilder msgBuilder = new StringBuilder(htmlBody);
            msgBuilder.AppendLine("<br>");
            msgBuilder.AppendLine(ReadSignature());

            oMsg.HTMLBody = msgBuilder.ToString();


            //Add an attachment.

            int iPosition = oMsg.Body.Length + 1;
            const int iAttachType = (int)OlAttachmentType.olByValue;


            //now attached the file
            oMsg.Attachments.Add(path, iAttachType, iPosition, fileName);



            //Subject line
            oMsg.Subject = subject;
            oMsg.ReadReceiptRequested = true;
            oMsg.OriginatorDeliveryReportRequested = true;

            // Add a recipient.
            Recipients oRecips = oMsg.Recipients;
            // Change the recipient in the next line if necessary.
            foreach (string address in toEmailAddress.Split(';'))
            {
                if (string.IsNullOrWhiteSpace(address)) continue;
                oRecips.Add(address);
            }

            oRecips.ResolveAll();
            oMsg.Display(isModel);

            if (File.Exists(path)) File.Delete(path);
            return true;
        }

        public bool SendMail(string fromAddress, string toEmailAddress, string ccEmailAddress, string subject, string htmlBody, string fileName, string path, bool isModel)
        {
            // Create the Outlook application.
            Application oApp = new Application();
            // Create a new mail item.
            MailItem oMsg = (MailItem)oApp.CreateItem(OlItemType.olMailItem);
            oMsg.BodyFormat = OlBodyFormat.olFormatHTML;
            // Set HTMLBody. 
            //add the body of the email

            StringBuilder msgBuilder = new StringBuilder(htmlBody);
            msgBuilder.AppendLine("<br>");
            msgBuilder.AppendLine(ReadSignature());

            oMsg.HTMLBody = msgBuilder.ToString();


            //Add an attachment.

            int iPosition = oMsg.Body.Length + 1;
            const int iAttachType = (int)OlAttachmentType.olByValue;


            //now attached the file
            oMsg.Attachments.Add(path, iAttachType, iPosition, fileName);



            //Subject line
            oMsg.Subject = subject;
            oMsg.ReadReceiptRequested = true;
            oMsg.OriginatorDeliveryReportRequested = true;

            // Add a recipient.
            Recipients oRecips = oMsg.Recipients;
            // Change the recipient in the next line if necessary.
            foreach (string address in toEmailAddress.Split(';'))
            {
                if (string.IsNullOrWhiteSpace(address)) continue;
                oRecips.Add(address);
            }


            oMsg.CC = ccEmailAddress;

            oRecips.ResolveAll();
            oMsg.Display(isModel);

            if (File.Exists(path)) File.Delete(path);
            return true;
        }

        private string ReadSignature()
        {
            string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Signatures";
            string signature = string.Empty;
            DirectoryInfo diInfo = new DirectoryInfo(appDataDir);

            if (diInfo.Exists)
            {
                FileInfo[] fiSignature = diInfo.GetFiles("*.htm");

                if (fiSignature.Length > 0)
                {
                    StreamReader sr = new StreamReader(fiSignature[0].FullName, Encoding.Default);
                    signature = sr.ReadToEnd();

                    if (!string.IsNullOrEmpty(signature))
                    {
                        string fileName = fiSignature[0].Name.Replace(fiSignature[0].Extension, string.Empty);
                        signature = signature.Replace(fileName + "_files/", appDataDir + "/" + fileName + "_files/");
                    }
                }
            }
            return signature;
        }


        public bool SendMail(string[] emailAddress, string[] attachments)
        {
            // Create the Outlook application.
            Application oApp = new Application();
            // Create a new mail item.
            MailItem oMsg = (MailItem)oApp.CreateItem(OlItemType.olMailItem);
            oMsg.BodyFormat = OlBodyFormat.olFormatHTML;
            // Set HTMLBody. 
            //add the body of the email

            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine("Dear Sir,").Append("<br>");
            strBuilder.AppendLine("Please find the attached for your information and necessary action.").Append("<br>");
            strBuilder.AppendLine(string.Empty).Append("<br>");
            strBuilder.AppendLine(string.Empty).Append("<br>");
            strBuilder.AppendLine("Thank you.").Append("<br>");
            strBuilder.AppendLine("Regards,").Append("<br>");
            strBuilder.AppendLine("Rudolf").Append("<br>");
            oMsg.HTMLBody = strBuilder.ToString();

            //Add an attachment.
            const string sDisplayName = "MyAttachment";
            int iPosition = oMsg.Body.Length + 1;
            const int iAttachType = (int)OlAttachmentType.olByValue;
            //now attached the file
            foreach (string attachement in attachments)
            {
                oMsg.Attachments.Add(attachement, iAttachType, iPosition, sDisplayName);
            }

            //long maxSiz= oMsg.Attachments.Cast<Attachment>().Aggregate<Attachment, long>(0, (current, attachment) => current + attachment.Size);
            //if (maxSiz > 10000) return ;
            //Subject line
            oMsg.Subject = "Your Subject will go here.";
            oMsg.ReadReceiptRequested = true;
            oMsg.OriginatorDeliveryReportRequested = true;

            // Add a recipient.
            Recipients oRecips = oMsg.Recipients;
            // Change the recipient in the next line if necessary.
            foreach (string addressList in emailAddress)
            {
                string[] a = addressList.Split(';');
                foreach (string address in a)
                {
                    if (string.IsNullOrWhiteSpace(address)) continue;
                    oRecips.Add(address);

                }
            }
            oRecips.ResolveAll();
            oMsg.Display(true);


            // Send.
            // oMsg.Send();
            // Clean up.
            //oRecip = null;
            /*
                            oRecips = null;

                            oMsg = null;
                            oApp = null;*/
            return true;
        }

        public bool SendMail(string to, string cc, string bcc, string[] attachments)
        {
            // Create the Outlook application.
            Application oApp = new Application();
            // Create a new mail item.
            MailItem oMsg = (MailItem)oApp.CreateItem(OlItemType.olMailItem);
            oMsg.BodyFormat = OlBodyFormat.olFormatHTML;
            // Set HTMLBody. 
            //add the body of the email

            StringBuilder strBuilder = new StringBuilder();
            strBuilder.AppendLine("Dear Sir,").Append("<br>");
            strBuilder.AppendLine("Please find the attached for your information and necessary action.").Append("<br>");
            strBuilder.AppendLine(string.Empty).Append("<br>");
            strBuilder.AppendLine(string.Empty).Append("<br>");
            strBuilder.AppendLine("Thank you.").Append("<br>");
            strBuilder.AppendLine("Regards,").Append("<br>");
            strBuilder.AppendLine("Rudolf").Append("<br>");
            oMsg.HTMLBody = strBuilder.ToString();

            //Add an attachment.
            const string sDisplayName = "MyAttachment";
            int iPosition = oMsg.Body.Length + 1;
            const int iAttachType = (int)OlAttachmentType.olByValue;
            //now attached the file
            foreach (string attachement in attachments)
            {
                oMsg.Attachments.Add(attachement, iAttachType, iPosition, sDisplayName);
            }

            //long maxSiz= oMsg.Attachments.Cast<Attachment>().Aggregate<Attachment, long>(0, (current, attachment) => current + attachment.Size);
            //if (maxSiz > 10000) return ;
            //Subject line
            oMsg.Subject = "Your Subject will go here.";
            oMsg.ReadReceiptRequested = true;
            oMsg.OriginatorDeliveryReportRequested = true;

            oMsg.To = to;
            oMsg.CC = cc;
            oMsg.BCC = bcc;

            oMsg.Display(true);


            // Send.
            // oMsg.Send();
            // Clean up.
            //oRecip = null;
            /*
                            oRecips = null;

                            oMsg = null;
                            oApp = null;*/
            return true;
        }
    }

    public class MailBeeEmailService : IEmailService
    {
        public MailBeeEmailService()
        {
           

        }
        public bool SendMail(string fromAddress, string toEmailAddress, string subject, string htmlBody, StreamAttachment attachment)
        {
            throw new NotImplementedException();
        }

        public bool SendMail(string fromAddress, string toEmailAddress, string subject, string htmlBody)
        {
            throw new NotImplementedException();
        }

        public bool SendMail(string[] emailAddress, string[] attachments)
        {
            throw new NotImplementedException();
        }

        public bool SendMail(string to, string cc, string bcc, string[] attachments)
        {
            throw new NotImplementedException();
        }

        public bool SendMail(string fromAddress, string toEmailAddress, string subject, string htmlBody, string fileName, string path, bool isModel)
        {
            throw new NotImplementedException();
        }

        public bool SendMail(string fromAddress, string toEmailAddress, string ccEmailAddress, string subject, string htmlBody, string fileName, string path, bool isModel)
        {
            throw new NotImplementedException();
        }
    }
}