using NotificationService.Model;
using NotificationService.Result;
using System;

namespace NotificationService.Sender
{
    public class BulkMailSender : BaseMessageSender<BulkMail, MessageResult<BulkMail>>
    {
        protected override MessageResult<BulkMail> SendMessage(BulkMail message)
        {
            bool isSend = false;
            MessageResult<BulkMail> result = new MessageResult<BulkMail>(message);
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
                              message.FromEmail
                             );

                    if (isSend)
                    {
                        result.SendDateTime = DateTime.Today;
                        result.Message = message;
                        result.IsSuccesful = true;
                        Logger.EnterLog(LogType.Trace, "Bulk mail is succesfully sent for " + Common.CreateParameterString(message.EmailId), null, "BulkMail");
                    }
                }
                else
                {
                    Logger.EnterLog(LogType.Error, "Bulk Mail isn't sent for " + Common.CreateParameterString(message.RecipientEmail) + " because of recipientEmail is null", null, "BulkMail");
                }
            }
            catch (System.Exception ex)
            {
                result.IsSuccesful = false;
                result.Exception = ex;
                result.Message = message;
                Logger.EnterLog(LogType.Error, "Bulk mail isn't sent for " + Common.CreateParameterString(message.EmailId), ex, "BulkMail");
            }
            return result;
        }

        /// <summary>
        /// /// /// SendMessage methodu çalıştıktan sonra bulk mail gönderim işlemi başarılıysa tabloyu güncelliyoruz.
        /// </summary>
        /// <param name="result"></param>
        protected override void AfterProcessResult(MessageResult<BulkMail> result)
        {
            try
            {
                if (result.IsSuccesful)
                {

                    Logger.EnterLog(LogType.Trace, " succesfully updated on ... for " + Common.CreateParameterString(result.Message.Id), null, "BulkMail");
                }
                else
                {
                    Logger.EnterLog(LogType.Error, " mail isn't sent" + Common.CreateParameterString(result.Message.EmailId), null, "BulkMail");
                }

            }
            catch (Exception ex)
            {
                result.Exception = ex;
                Logger.EnterLog(LogType.Error, "Bulk mail record status not updated on ... because of mail isn't sent" + Common.CreateParameterString(result.Message.EmailId), ex, "BulkMail");
            }
        }

    }
}
