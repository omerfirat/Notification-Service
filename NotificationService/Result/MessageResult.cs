using NotificationService.Model;
using System;

namespace NotificationService.Result
{
    /// <summary>
    /// Mesaj gönderimi sonrasında oluşacak olan sonuç tipi
    /// Bastract değildir.Gerekirse türetilerek farklılaştırılabilir.
    /// </summary>
    /// <typeparam name="TMes">Gönderilen mesaj tipi</typeparam>
    public  class MessageResult<TMes> where TMes:BaseMessage
    {
        /// <summary>
        /// Gönderilmiş olan mesaj
        /// </summary>
        public TMes Message { get; set; }
        public Exception Exception { get; set; }
        public bool IsSuccesful { get; set; } = false;
        /// <summary>
        /// Mesaj gönderim zamanı
        /// </summary>
        public DateTime SendDateTime { get; set; }

        public int ElapsedMiliseconds { get; set; }

        public MessageResult(TMes mes)
        {
            Message = mes;
        }

    }
}
