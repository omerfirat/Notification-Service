using NotificationService.Model;
using NotificationService.Result;
using System;

namespace NotificationService.Request
{
    public abstract class BaseMessageRequest<TMes, TRes> : IMessageRequest<TMes>
          where TMes : BaseMessage
          where TRes : MessageResult<TMes>
    {
        public void Request(TMes mes)
        {
            TRes result = null;
            try
            {
                result = RequestMessage(mes);
            }
            catch (Exception ex)
            {
                result.Exception = ex;
                result.IsSuccesful = false;
                throw;
            }
            finally
            {
                AfterProcessResult(result);
            }
        }

        protected abstract TRes RequestMessage(TMes message);

        protected virtual void AfterProcessResult(TRes result)
        {
            if (result.IsSuccesful)
            {
                //result.Message.Id - statu 2

            }
            else
            {
                //result.Message.Id - statu 3

            }
        }

    }
}
