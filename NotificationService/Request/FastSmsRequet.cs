using NotificationService.Model;
using NotificationService.Result;
using System;

namespace NotificationService.Request
{
    public class FastSmsRequet : BaseMessageRequest<Sms, SmsResult>
    {
        protected override SmsResult RequestMessage(Sms message)
        {
            SmsResult result = new SmsResult(message);
            try
            {
                string requestResult = "";
                result.IsSuccesful = true;
                result.Message = message;              
                result.Message.Id = requestResult;
            }
            catch (Exception ex)
            {
                result.IsSuccesful = false;
                result.Exception = ex;
                result.Message = message;                
                Logger.EnterLog(LogType.Error, "Fast Sms not request for  " + Common.CreateParameterString(message.SmsId, message.Id), ex, "FastSmsRequest", "FastSmsRequest->RequestMessage");
            }

            return result;
        }

        protected override void AfterProcessResult(SmsResult result)
        {
            try
            {
                if (result.IsSuccesful == true)
                {
                   
                }
            }

            catch (Exception ex)
            {
                result.Exception = ex;
                Logger.EnterLog(LogType.Error, "Fast Sms status and error code not updated  " + Common.CreateParameterString(result.Message.SmsId, result.Message.Id), ex, "FastSmsRequest", "FastSmsRequest->AfterProcessResult");
            }
        }
    }
}
