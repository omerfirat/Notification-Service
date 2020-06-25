using NotificationService.Model;
using NotificationService.Request;
using NotificationService.Sender;

namespace NotificationService
{
    public class MessageInstance
    {
        /// <summary>
        /// Mesaj tipine göre gönderici classı oluşturur.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IMessageSender<T> GetInstanceSender<T>() where T : BaseMessage
        {
            if (typeof(T) == typeof(Mail))
            {
                return new MailSender() as IMessageSender<T>;
            }

            else if (typeof(T) == typeof(Sms))
            {
                return new SmsSender() as IMessageSender<T>;
            }
            else if (typeof(T) == typeof(BulkMail))
            {
                return new BulkMailSender() as IMessageSender<T>;
            }
            else if (typeof(T) == typeof(BulkSms))
            {
                return new BulkSmsSender() as IMessageSender<T>;
            }
            return null;

        }

        public static IMessageRequest<T> GetInstanceRequest<T>() where T : BaseMessage
        {
            if (typeof(T) == typeof(Sms))
            {
                return new FastSmsRequet() as IMessageRequest<T>;
            }
            else if (typeof(T) == typeof(BulkSms))
            {
                return new BulkSmsRequest() as IMessageRequest<T>;
            }
            return null;

        }
    }
}
