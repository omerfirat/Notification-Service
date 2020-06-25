using NotificationService.Model;
using NotificationService.Result;
using System;

namespace NotificationService.Request
{
    public class BulkSmsRequest : BaseMessageRequest<BulkSms, BulkSmsResult>
    {
        protected override BulkSmsResult RequestMessage(BulkSms message)
        {
            BulkSmsResult result = new BulkSmsResult(message);
            try
            {
                string resultMsgStatus = "";
                result.Message.Id = resultMsgStatus;
                result.IsSuccesful = true;
            }
            catch (Exception ex)
            {
                result.IsSuccesful = false;
                result.Exception = ex;
                result.Message = message;
                Logger.EnterLog(LogType.Error, "Bulk Sms note request for  " + Common.CreateParameterString(message.CombinedSmsId, message.Id), ex, "BulkSmsRequest", "BulkSmsRequest->RequestMessage");
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

            }
            catch (Exception ex)
            {               
                Logger.EnterLog(LogType.Error, "Bulk Sms status and error code not updated  " + Common.CreateParameterString(result.Message.CombinedSmsId, result.Message.Id), ex, "BulkSmsRequest", "BulkSmsRequest->AfterProcessResult");
            }
        }
    }
}
