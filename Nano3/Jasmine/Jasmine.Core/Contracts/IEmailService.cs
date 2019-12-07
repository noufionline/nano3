using Jasmine.Core.Common;

namespace Jasmine.Core.Contracts
{
    public interface IEmailService
    {
        bool SendMail(string fromAddress, string toEmailAddress, string subject, string htmlBody, StreamAttachment attachment);
        bool SendMail(string fromAddress, string toEmailAddress, string subject, string htmlBody);
        bool SendMail(string[] emailAddress, string[] attachments);
        bool SendMail(string to, string cc, string bcc, string[] attachments);
        bool SendMail(string fromAddress, string toEmailAddress, string subject, string htmlBody, string fileName, string path, bool isModel);
        bool SendMail(string fromAddress, string toEmailAddress, string ccEmailAddress, string subject, string htmlBody, string fileName, string path, bool isModel);

    }

    public interface IJasmineEmailService
    {
       // bool SendMail(string fromAddress, string toEmailAdress, string subject, string htmlBody, params Guid[] fileids);
    }
}