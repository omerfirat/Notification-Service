using NotificationService.Model;


namespace NotificationService.Request
{
    public interface IMessageRequest<TMes> where TMes : BaseMessage
    {
        void Request(TMes mes);
    }
}
