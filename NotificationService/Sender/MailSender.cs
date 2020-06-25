using NotificationService.Model;
using NotificationService.Result;
using System;

namespace NotificationService.Sender
{
    public class MailSender : BaseMessageSender<Mail, MailResult>
    {
        protected override MailResult SendMessage(Mail message)
        {
            bool isSend = false;
            MailResult result = new MailResult(message);
            try
            {
                if (!string.IsNullOrEmpty(message.RecipientEmail))
                {

                    isSend = MailHelper.SendMail(message.EmailId,
                                              message.RecipientEmail,
                                              message.Subject,
                                              message.Message,
                                              message.Attachment,
                                              message.BCC,
                                              message.FromDisplayName,
                                              message.FromEmail);


                    if (isSend)
                    {
                        result.SendDateTime = DateTime.Today;
                        result.Message = message;
                        result.IsSuccesful = true;
                        result.Message.Id = message.EmailId;
                        Logger.EnterLog(LogType.Trace, "Mail is succesfully sent for " + GetCreateParameter(message) + " ElapsedMiliseconds->" + result.ElapsedMiliseconds, null, "Mail");
                    }
                    else
                    {
                        Logger.EnterLog(LogType.Error, "Mail isn't sent for " + GetCreateParameter(message), null, "Mail");
                    }
                }
                else
                {
                    Logger.EnterLog(LogType.Error, "Mail isn't sent for  " + GetCreateParameter(message) + " because of recipientEmail is null ", null, "Mail");
                }
            }
            catch (System.Exception ex)
            {
                result.IsSuccesful = false;
                result.Exception = ex;
                result.Message = message;
                Logger.EnterLog(LogType.Error, "Mail isn't for " + GetCreateParameter(message), ex, "Mail");
            }

            return result;
        }

        /// <summary>
        /// SendMessage methodu çalıştıktan sonra mail gönderim işlemi başarılıysa tabloyu güncelliyoruz.
        /// </summary>
        /// <param name="result"></param>
        protected override void AfterProcessResult(MailResult result)
        {
            try
            {
                if (result.IsSuccesful)
                {

                }
                else
                {
                    if (!string.IsNullOrEmpty(result.Message.RecipientEmail))
                        Logger.EnterLog(LogType.Error, "Mail record status not updated   on table ... because of mail isn't sent " + GetCreateParameter(result.Message), null, "Mail");
                    else
                        Logger.EnterLog(LogType.Error, "Mail record status not updated  on table ... because of RecipientEmail is null " + GetCreateParameter(result.Message), null, "Mail");
                }
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                Logger.EnterLog(LogType.Error, "Mail record status not updated  on table ... because o an error " + GetCreateParameter(result.Message), ex, "Mail");
            }
        }

        public string GetCreateParameter(Mail mail)
        {
            return Common.CreateParameterString(mail.EmailId, mail.Attachment,
                                                 mail.BCC, mail.FromDisplayName,
                                                 mail.FromEmail, mail.Id,
                                                 mail.RecipientDisplayName, mail.RecipientEmail,
                                                 mail.Subject);

        }
    }
}
