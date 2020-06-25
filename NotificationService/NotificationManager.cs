using NotificationService.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NotificationService
{
    public class NotificationManager
    {
        public static void SendMessage<T>(List<T> messages) where T : BaseMessage
        {
            Parallel.ForEach(messages, new ParallelOptions { MaxDegreeOfParallelism = ConfigReader.ThreadCount }, message =>
            {
                var request = MessageInstance.GetInstanceSender<T>();
                request.Send(message);            
            });
        }

        public static void SendMessage<T>(T message) where T : BaseMessage
        {
            var sender = MessageInstance.GetInstanceSender<T>();
            sender.Send(message);
        }

        public static void RequestMessage<T>(T message) where T : BaseMessage
        {
            var request = MessageInstance.GetInstanceRequest<T>();
            request.Request(message);
        }

        public static void RequestMessage<T>(List<T> messages) where T : BaseMessage
        {
            Parallel.ForEach(messages, new ParallelOptions { MaxDegreeOfParallelism = ConfigReader.ThreadCount }, message =>
            {
                var request = MessageInstance.GetInstanceRequest<T>();
                request.Request(message);
            });

        }

    }

}

