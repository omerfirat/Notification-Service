using NotificationService.Model;
using System;
using NotificationService.Result;

namespace NotificationService.Sender
{
    /// <summary>
    /// Mesaj gönderimi yapcak classlar için base classdır
    /// </summary>
    /// <typeparam name="TMes">Gönderilecek mesaj tipi</typeparam>
    /// <typeparam name="TRes">Göderim sonrası oluşacak mesaj sonuç tipi</typeparam>
    public abstract class BaseMessageSender<TMes,TRes> :IMessageSender<TMes>
        where TMes :BaseMessage
        where TRes :MessageResult<TMes>
    {
        /// <summary>
        /// Gönderim işini yapan, dışardan erişilebilen methodtur.
        /// Gönderim işini türeyen classlara yaptırır. Gönderim öncesi ve sonrası ortak yapılacak işlemler burada yapılabilir.
        /// </summary>
        /// <param name="message"></param>
        public int Send(TMes message)
        {
            TRes result = null;
            try
            {
                DateTime startTime = DateTime.Now;
                result = SendMessage(message);
                result.SendDateTime = DateTime.Now;
                result.ElapsedMiliseconds = (result.SendDateTime - startTime).Milliseconds;
            }
            catch (Exception e)
            {
                result.Exception = e;
                result.IsSuccesful = false;
            }
            finally
            {
                AfterProcessResult(result);
            }

            return 1;
        }

        /// <summary>
        /// Mesaj gönderme işlemini yapacak olan ve türeyen classlar tarafından yazılması gerekli olan methodtur.
        /// Bu method dışardan çağrılamaz.Send methodu içinden çağrılabilir (protected)
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        protected abstract TRes SendMessage(TMes message);

        /// <summary>
        /// Mesaj işlendikten sonra oluşan sonucu işler.
        /// Türeyen classlar isterse bu methodu override edip farklı şekilde result process edebilir (virtual  )
        /// </summary>
        /// <param name="result"></param>
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
