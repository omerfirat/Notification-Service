using NotificationService.Model;
using NotificationService.Result;
using System;

namespace NotificationService.Sender
{
    public class BulkSmsSender : BaseMessageSender<BulkSms, BulkSmsResult>
    {
        
        protected override BulkSmsResult SendMessage(BulkSms message)
        {
            string resultCode = "";
            BulkSmsResult result = new BulkSmsResult(message);
            try
            {
                if (!string.IsNullOrEmpty(message.MessageContent))
                {
                    // Send Sms

                    if (resultCode.Length == 36)
                    {
                        result.Message.Id = resultCode;
                        result.IsSuccesful = true;
                        Logger.EnterLog(LogType.Info, "Bulk Sms is succesfully sent for " + Common.CreateParameterString(resultCode, message.CombinedSmsId), null, "BulkSms");
                    }
                    else
                    {
                        result.Message.Id = resultCode;
                        result.IsSuccesful = false;
                        result.ErrorCode = resultCode;
                        Logger.EnterLog(LogType.Error, message.MsgSpecialId.ToString() + " paketi hata verdi! Hata Kodu: " + resultCode + Common.CreateParameterString(resultCode, message.CombinedSmsId), null, "BulkSms");
                    }
                }
                else
                {
                    result.Message.Id = resultCode;
                    result.IsSuccesful = false;
                    Logger.EnterLog(LogType.Error, "Bulk Sms isn't sent for " + message.CombinedSmsId + " SmsId because of message content is null", null, "BulkSms");
                }
            }
            catch (Exception ex)
            {
                result.IsSuccesful = false;
                result.Message.Id = resultCode;
                Logger.EnterLog(LogType.Error, ex.Message + "Bulk SMS Web Service Hatası", ex, "BulkSms");
            }

            return result;
        }

        protected override void AfterProcessResult(BulkSmsResult result)
        {
            try
            {
                if (result.IsSuccesful)
                {
                   
                }
                else
                {
                    
                    Logger.EnterLog(LogType.Error, "Bulk Sms not updated for  " + result.Message.CombinedSmsId + " SmsId Result Code: " + result.Message.Id, null, "BulkSms");
                }
            }
            catch (Exception ex)
            {
                Logger.EnterLog(LogType.Error, ex.Message + " " + Common.CreateParameterString(result.Message.CombinedSmsId), ex, "BulkSms");
            }

        }
    }
}
